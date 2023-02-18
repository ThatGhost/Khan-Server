using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.Collections;
using Unity.Networking.Transport;
using Unity.Networking.Transport.Utilities;
using Zenject;

using Khan_Shared.Networking;
using static Networking.Core.GameServer;

namespace Networking.Core
{
    public class GameServer : MonoBehaviour, IGameServer
    {
        private NetworkDriver m_networkDriver;
        private NativeList<NetworkConnection> m_connections;
        private NetworkPipeline m_updatePipeline;
        private NetworkPipeline m_relaiblePipeline;

        private int m_tick = 0;

        public delegate void OnServerTick(int tick);
        public static OnServerTick onServerTick;

        public delegate void OnClientConnect(int internalId);
        public static OnClientConnect onClientConnect;

        public delegate void OnClientDisconnect(int internalId);
        public static OnClientDisconnect onClientDisconnect;

        [Inject] private readonly IMessageQueue m_messageQueue;

        public int Tick
        {
            get
            {
                return m_tick;
            }
        }

        private void Start()
        {
            m_networkDriver = NetworkDriver.Create();
            m_updatePipeline = m_networkDriver.CreatePipeline();
            m_relaiblePipeline = m_networkDriver.CreatePipeline(typeof(ReliableSequencedPipelineStage));

            NetworkEndPoint endpoint = NetworkEndPoint.AnyIpv4;
            endpoint.Port = NetworkingCofigurations.g_serverPort;
            if (m_networkDriver.Bind(endpoint) == 0)
            {
                m_networkDriver.Listen();
            }
            else
            {
                Debug.LogError($"Failed to bind to port {endpoint.Port}");
            }
            m_connections = new NativeList<NetworkConnection>(NetworkingCofigurations.g_maxPlayers, Allocator.Persistent);
            m_messageQueue.Init();
        }

        private void FixedUpdate()
        {
            m_networkDriver.ScheduleUpdate().Complete();
            cleanUpConnections();
            acceptNewConnections();

            readMessages();
            m_messageQueue.InvokeMessages();

            if (GameServer.onServerTick != null)
                GameServer.onServerTick.Invoke(m_tick);

            writeMessages();
            m_tick++;
        }

        private void OnDestroy()
        {
            if (m_networkDriver.IsCreated)
            {
                m_networkDriver.Dispose();
                m_connections.Dispose();
            }
        }

        private void cleanUpConnections()
        {
            for (int i = 0; i < m_connections.Length; i++)
            {
                if (!m_connections[i].IsCreated)
                {
                    m_connections.RemoveAtSwapBack(i);
                    --i;
                }
            }
        }

        private void acceptNewConnections()
        {
            NetworkConnection conn;
            while ((conn = m_networkDriver.Accept()) != default(NetworkConnection))
            {
                m_connections.Add(conn);
                StartCoroutine(clientConnectWhaitTime(conn.InternalId));
            }
        }

        private IEnumerator clientConnectWhaitTime(int conn)
        {
            yield return new WaitForSeconds(0.5f);
            Debug.Log($"Client connected {conn}");
            if (GameServer.onClientConnect != null)
               GameServer.onClientConnect.Invoke(conn);
        }

        private void readMessages()
        {
            DataStreamReader stream;
            for (int i = 0; i < m_connections.Length; i++)
            {
                if (!m_connections.IsCreated)
                    continue;

                NetworkEvent.Type cmd;
                while ((cmd = m_networkDriver.PopEventForConnection(m_connections[i], out stream)) != NetworkEvent.Type.Empty)
                {
                    if (cmd == NetworkEvent.Type.Disconnect)
                    {
                        //GameServer.onClientDisconnect.Invoke(m_connections[i].InternalId);
                        continue;
                    }

                    if (cmd == NetworkEvent.Type.Data)
                    {
                        m_messageQueue.ReadMessage(ref stream, i);
                    }
                }
            }
        }

        private void writeMessages()
        {
            for (int i = 0; i < m_connections.Length; i++)
            {
                if (m_connections[i] == default(NetworkConnection))
                    continue;

                try
                {
                    //unrelaible update layer
                    if (m_messageQueue.OutQueueSize(i) > 0)
                    {   
                        int code = m_networkDriver.BeginSend(m_updatePipeline, m_connections[i], out var writer);

                        if (code == 0) m_messageQueue.DequeueMessages(ref writer, m_connections[i].InternalId);
                        else throw new System.Exception($"Cant send data to server, writer exited with code {code}");

                        m_networkDriver.EndSend(writer);
                    }

                    //relailability layer
                    if (m_messageQueue.RelaibleQueueSize(i) > 0)
                    {  
                        int code = m_networkDriver.BeginSend(m_relaiblePipeline, m_connections[i], out var writer);

                        if (code == 0) m_messageQueue.DequeuRelaibleMessages(ref writer, m_connections[i].InternalId);
                        else throw new System.Exception($"Cant send data to server, writer exited with code {code}");

                        m_networkDriver.EndSend(writer);
                    }
                }
                catch (System.Exception err)
                {
                    m_connections[i] = default(NetworkConnection);
                    Debug.Log($"connection {i} timed out\n with err {err}");
                }
            }
        }
    }
}
