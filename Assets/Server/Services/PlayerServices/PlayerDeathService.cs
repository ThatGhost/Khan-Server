using Khan_Shared.Networking;

using System;
using System.Collections;
using UnityEngine;

using Zenject;

using ConnectionId = System.Int32;

namespace Server.Services
{
    public class PlayerDeathService : IPlayerDeathService, IInitializable, IDisposable
    {
        [Inject] private readonly SignalBus _signalBus;
        [Inject] private readonly IPlayersController _playersController;
        [Inject] private readonly IMonoHelper _monoHelper;
        [Inject] private readonly IMessagePublisher _messagePublisher;
        [Inject(Id = "spawnPoint")] private readonly Transform spawnpoint;

        private readonly float respawnCooldown = 5f;

        public void Dispose()
        {
            _signalBus.TryUnsubscribe<OnDeathSignal>(x => onDeath(x.connectionId));
        }

        public void Initialize()
        {
            _signalBus.Subscribe<OnDeathSignal>(x => onDeath(x.connectionId));
        }

        private void onDeath(ConnectionId connectionId)
        {
            PlayerRefrenceObject? player = _playersController.getPlayer(connectionId);
            if (player == null) throw new Exception("err.services.player.notfound");


            // stop player controll -- position and spell controller to unactive
            player.Value._playerSpellController.Active = false;
            player.Value._playerPositionBehaviour.Active = false;
            player.Value._gameObject.SetActive(false);

            // send death message to client
            Message msg = new Message(MessageTypes.PlayerDeath, new object[] { (ushort)connectionId }, MessagePriorities.high, true);
            _messagePublisher.PublishGlobalMessage(msg);

            // respawn timer
            _monoHelper.StartCoroutine(deathCooldown(player.Value));
        }

        private IEnumerator deathCooldown(PlayerRefrenceObject player)
        {
            yield return new WaitForSeconds(respawnCooldown);
            respawn(player);
        }

        private void respawn(PlayerRefrenceObject player)
        {
            Debug.Log("respawn, "+ spawnpoint.position);
            // position to spawn location -- player position behaviour set
            player._gameObject.SetActive(true);
            player._playerPositionBehaviour.SetPositionAndVelocity(spawnpoint.position, Vector3.zero);

            // full hp and mana -- send add hp and mana signals with 1000 hp and mana as amount
            _signalBus.Fire(new OnHealthSignal() { connectionId = player._connectionId, amount = 100000 });
            _signalBus.Fire(new OnManaSignal() { connectionId = player._connectionId, amount = 100000 });

            // give back controll -- position and spell controller to active
            player._playerSpellController.Active = true;
            player._playerPositionBehaviour.Active = true;

            // send respawn message to client
            Message msg = new Message(MessageTypes.PlayerRespawn, new object[] {
                (ushort)player._connectionId,
                (float)spawnpoint.position.x,
                (float)spawnpoint.position.y,
                (float)spawnpoint.position.z,
                (float)0f
            }, MessagePriorities.high, true);
            _messagePublisher.PublishGlobalMessage(msg);
        }
    }
}
