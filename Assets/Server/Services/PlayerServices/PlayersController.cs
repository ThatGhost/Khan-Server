using System;
using System.Collections;
using System.Collections.Generic;
using Networking.Behaviours;
using UnityEngine;
using System.Linq;

using ConnectionId = System.Int32;

namespace Networking.Services
{
    public class PlayersController
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
    }
}
