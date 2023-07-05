using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Networking.Services
{
    public class PlayersVariableService : IPlayersVariableService
    {
        [Inject] private readonly IPlayersController m_playersController;

        public void routeHealth(GameObject[] playerObjects, int amount)
        {
            foreach (var playerObject in playerObjects)
            {
                PlayerRefrenceObject? playerRefrence = m_playersController.getPlayer(playerObject);
                if (playerRefrence == null) { Debug.LogError("PlayerNotFound"); continue; }

                playerRefrence.Value._playerVariableService.addHp(amount);
            }
        }

        public void routeHealth(GameObject[] playerObjects, int amount, int playerSpellId)
        {
            foreach (var playerObject in playerObjects)
            {
                PlayerRefrenceObject? playerRefrence = m_playersController.getPlayer(playerObject);
                if (playerRefrence == null) { Debug.LogError("PlayerNotFound"); continue; }

                if(!playerRefrence.Value._playerSpellController.playerOwnsSpell(playerSpellId))
                    playerRefrence.Value._playerVariableService.addHp(amount);
            }
        }

        public void routeMana(int playerSpellId, int amount)
        {
            foreach (var playerObject in m_playersController.getPlayers())
            {
                if (playerObject._playerSpellController.playerOwnsSpell(playerSpellId))
                    playerObject._playerVariableService.addMana(amount);
            }
        }

        public void onDeath(int connectionId)
        {
            //
        }
    }
}
