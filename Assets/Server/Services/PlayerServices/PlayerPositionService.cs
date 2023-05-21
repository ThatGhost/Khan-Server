using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Networking.Core;
using System.Linq;
using Khan_Shared.Networking;
using System;

namespace Networking.Services
{
    public class PlayerPositionService: IInitializable, ILateDisposable
    {
        [Inject] private readonly IMessagePublisher m_messagePublisher;
        [Inject] private readonly IPlayersController m_playersController;
        [Inject] private readonly IMonoHelper m_monoHelper;

        private readonly float timeBetweenReconciliation = 0.5f;

        public void Initialize()
        {
            GameServer.onServerTick += onTick;
            m_monoHelper.StartCoroutine(serverReconciliation());
        }

        public void LateDispose()
        {
            GameServer.onServerTick -= onTick;
        }

        public void onTick(int tick)
        {
            PlayerRefrenceObject[] players = m_playersController.getPlayers();

            foreach (var player in players)
            {
                foreach (var otherplayer in players.Where(p => p._connectionId != player._connectionId))
                {
                    Vector2 faceRotation = otherplayer._playerPositionBehaviour.FaceRotation;
                    object[] data = new object[]
                    {
                        (ushort)otherplayer._connectionId,
                        (float)otherplayer._gameObject.transform.position.x,
                        (float)otherplayer._gameObject.transform.position.y,
                        (float)otherplayer._gameObject.transform.position.z,
                        (float)faceRotation.x,
                        (float)faceRotation.y,
                    };
                    Message msg = new Message(MessageTypes.PositionData, data, MessagePriorities.medium);
                    m_messagePublisher.PublishMessage(msg, player._connectionId);
                }
            }
        }

        private IEnumerator serverReconciliation()
        {
            yield return new WaitForSecondsRealtime(timeBetweenReconciliation);
            sendServerReconciliation();
            m_monoHelper.StartCoroutine(serverReconciliation());
        }

        private void sendServerReconciliation()
        {
            PlayerRefrenceObject[] players = m_playersController.getPlayers();

            foreach (var player in players)
            {
                Vector3 playerPosition = player._gameObject.transform.position;
                DateTime now = DateTime.UtcNow;

                object[] data = new object[]
                {
                    (float)playerPosition.x,
                    (float)playerPosition.y,
                    (float)playerPosition.z,
                    (DateTime)now,
                };
                Message msg = new Message(MessageTypes.ServerReconciliation, data, MessagePriorities.high);
                m_messagePublisher.PublishMessage(msg, player._connectionId);
            }
        }
    }
}
