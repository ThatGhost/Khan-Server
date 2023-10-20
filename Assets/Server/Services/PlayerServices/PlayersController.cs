using System;
using System.Collections;
using System.Collections.Generic;
using Server.Behaviours;
using UnityEngine;
using System.Linq;
using Zenject;
using Khan_Shared.Magic;

using ConnectionId = System.Int32;

namespace Server.Services
{
    public class PlayersController: IPlayersController
    {
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
                playerRefrenceObject._playerVariableService = playerBehaviour.m_playerVariableService;
                playerRefrenceObject._playerVariableService.setup(connection);
                m_playerRefrences.Add(connection, playerRefrenceObject);
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

        public void removePlayer(ConnectionId connection)
        {
            m_playerRefrences.Remove(connection);
        }
    }
}
