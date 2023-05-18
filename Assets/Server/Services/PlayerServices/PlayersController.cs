using System;
using System.Collections;
using System.Collections.Generic;
using Networking.Behaviours;
using UnityEngine;
using System.Linq;
using Zenject;
using Khan_Shared.Magic;

using ConnectionId = System.Int32;

namespace Networking.Services
{
    public class PlayersController: IPlayersController
    {
        [Inject] private readonly ISpellInitializer m_spellInitializer;
        private Dictionary<ConnectionId, PlayerRefrenceObject> m_playerRefrences = new Dictionary<ConnectionId, PlayerRefrenceObject>();

        public void AddPlayer(PlayerBehaviour playerBehaviour, ConnectionId connection)
        {
            if(!m_playerRefrences.ContainsKey(connection))
            {
                PlayerRefrenceObject playerRefrenceObject = new PlayerRefrenceObject();
                playerRefrenceObject._connectionId = connection;
                playerRefrenceObject._gameObject = playerBehaviour.gameObject;
                playerRefrenceObject._playerBehaviour = playerBehaviour;
                playerRefrenceObject._playerPositionBehaviour = playerRefrenceObject._gameObject.GetComponent<PlayerPositionBehaviour>();
                playerRefrenceObject._playerSpellController = playerBehaviour.m_playerSpellController;
                m_playerRefrences.Add(connection, playerRefrenceObject);

                // give spells to player
                m_spellInitializer.InitializeSpells(connection);
            }
        }

        public PlayerRefrenceObject? getPlayer(ConnectionId connection)
        {
            if(m_playerRefrences.ContainsKey(connection))
            {
                return m_playerRefrences[connection];
            }
            return null;
        }

        public PlayerRefrenceObject? getPlayer(GameObject gameObject)
        {
            foreach (var player in m_playerRefrences)
            {
                if (player.Value._gameObject == gameObject) return player.Value;
            }
            return null;
        }

        public PlayerRefrenceObject[] getPlayers()
        {
            return m_playerRefrences.Select(p => p.Value).ToArray();
        }
    }
}
