using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Server.Core;
using Server.Behaviours;
using System.Linq;
using Khan_Shared.Networking;

namespace Server.Services
{
    public class ClientInitializerService : IInitializable, ILateDisposable
    {
        [Inject] private Transform g_root;
        [Inject(Id = "spawnPoint")] private Transform g_spawnPoint;
        [Inject] private readonly PlayerBehaviour.Factory m_playerFactory;
        [Inject] private readonly IPlayersController m_playersController;
        [Inject] private readonly IMessagePublisher m_messagePublisher;
        [Inject] private readonly ISpellInitializer m_spellInitializer;

        public void Initialize()
        {
            GameServer.onClientConnect += onClientConnect;
        }

        public void LateDispose()
        {
            GameServer.onClientConnect -= onClientConnect;
        }

        private void onClientConnect(int connection)
        {
            initializeMessageQueue(connection);

            PlayerBehaviour playerhook = m_playerFactory.Create();
            playerhook.gameObject.transform.SetParent(g_root);
            playerhook.gameObject.transform.position = g_spawnPoint.position; // TEMP
            m_playersController.AddPlayer(playerhook, connection);

            sendHandShake(connection);
            sendNewClientOtherClients(connection);
            sendOtherClientsNewClient(connection);
            m_spellInitializer.InitializeSpells(connection);
        }

        private void initializeMessageQueue(int newconnection)
        {
            Message msg = new Message(MessageTypes.Default);
            m_messagePublisher.PublishMessage(msg, newconnection);
        }

        private void sendNewClientOtherClients(int newconnection)
        {
            PlayerRefrenceObject[] otherPlayers = m_playersController.getPlayers().Where(p => p._connectionId != newconnection).ToArray();
            foreach (var player in otherPlayers)
            {
                object[] data = new object[]
                {
                    (ushort)player._connectionId,
                    (float)player._gameObject.transform.position.x,
                    (float)player._gameObject.transform.position.y,
                    (float)player._gameObject.transform.position.z,
                };
                Message msg = new Message(MessageTypes.SpawnPlayer, data, MessagePriorities.high, true);
                m_messagePublisher.PublishMessage(msg, newconnection);
            }
        }

        private void sendOtherClientsNewClient(int newconnection)
        {
            PlayerRefrenceObject? newPlayer = m_playersController.getPlayer(newconnection);
            if (newPlayer == null)
                throw new System.Exception("new player was not created!");

            PlayerRefrenceObject[] otherPlayers = m_playersController.getPlayers().Where(p => p._connectionId != newconnection).ToArray();
            foreach (var player in otherPlayers)
            {
                object[] data = new object[]
                {
                    (ushort)newPlayer.Value._connectionId,
                    (float)newPlayer.Value._gameObject.transform.position.x,
                    (float)newPlayer.Value._gameObject.transform.position.y,
                    (float)newPlayer.Value._gameObject.transform.position.z,
                };
                Message msg = new Message(MessageTypes.SpawnPlayer, data, MessagePriorities.high, true);
                m_messagePublisher.PublishMessage(msg, player._connectionId);
            }
        }

        private void sendHandShake(int newconnection)
        {
            Message handshakeMessage = new Message(MessageTypes.HandShake, new object[] {(ushort)newconnection}, MessagePriorities.high, true);
            m_messagePublisher.PublishMessage(handshakeMessage, newconnection);
        }
    }
}