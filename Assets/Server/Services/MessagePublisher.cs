using System.Collections;
using System;
using System.Collections.Generic;

using Khan_Shared.Networking;
using Networking.Core;
using Zenject;

namespace Networking.Services
{
    public class MessagePublisher : IMessagePublisher
    {
        [Inject] private readonly IMessageQueue m_messageQueue;


        private Dictionary<int,Queue<Message>> m_highPrioQueue = new Dictionary<int,Queue<Message>>();
        private Dictionary<int,Queue<Message>> m_mediumPrioQueue = new Dictionary<int,Queue<Message>>();
        private Dictionary<int,Queue<Message>> m_lowPrioQueue = new Dictionary<int,Queue<Message>>();
        private List<int> m_connections = new List<int>();
        private readonly int maxBigMessageSize = 512;

        private MessagePublisher()
        {
            GameServer.onServerTick += onServerTick;
        }

        private void onServerTick(int tick)
        {
            foreach (var connection in m_connections)
            {
                int bigMessageSize = 0;
                List<Message> messages = new List<Message>();

                while (m_highPrioQueue[connection].Count > 0)
                {
                    int newBigMessageSize = bigMessageSize +
                        NetworkingCofigurations.getSizeOfMessage(m_highPrioQueue[connection].Peek());
                    if (newBigMessageSize > maxBigMessageSize) break;

                    bigMessageSize = newBigMessageSize;
                    messages.Add(m_highPrioQueue[connection].Dequeue());
                }

                while (m_mediumPrioQueue[connection].Count > 0)
                {
                    int newBigMessageSize = bigMessageSize +
                        NetworkingCofigurations.getSizeOfMessage(m_highPrioQueue[connection].Peek());
                    if (newBigMessageSize > maxBigMessageSize) break;

                    bigMessageSize = newBigMessageSize;
                    messages.Add(m_mediumPrioQueue[connection].Dequeue());
                }

                while (m_lowPrioQueue[connection].Count > 0)
                {
                    int newBigMessageSize = bigMessageSize +
                        NetworkingCofigurations.getSizeOfMessage(m_lowPrioQueue[connection].Peek());
                    if (newBigMessageSize > maxBigMessageSize) break;

                    bigMessageSize = newBigMessageSize;
                    messages.Add(m_lowPrioQueue[connection].Dequeue());
                }

                m_messageQueue.PublishMessages(messages.ToArray(), connection);
            }
        }

        public void PublishMessage(Message msg, int connection)
        {
            if (!m_connections.Contains(connection))
            {
                m_connections.Add(connection);
                m_highPrioQueue.Add(connection, new Queue<Message>());
                m_mediumPrioQueue.Add(connection, new Queue<Message>());
                m_lowPrioQueue.Add(connection, new Queue<Message>());
            }

            switch (msg.Priority)
            {
                case MessagePriorities.high:
                    m_highPrioQueue[connection].Enqueue(msg);
                    break;
                case MessagePriorities.medium:
                    m_mediumPrioQueue[connection].Enqueue(msg);
                    break;
                case MessagePriorities.low:
                    m_lowPrioQueue[connection].Enqueue(msg);
                    break;
            }
        }

        public void PublishGlobalMessage(Message msg)
        {
            switch (msg.Priority)
            {
                case MessagePriorities.high:
                    foreach (KeyValuePair<int, Queue<Message>> conn in m_highPrioQueue)
                    {
                        conn.Value.Enqueue(msg);
                    }
                    break;
                case MessagePriorities.medium:
                    foreach (KeyValuePair<int, Queue<Message>> conn in m_mediumPrioQueue)
                    {
                        conn.Value.Enqueue(msg);
                    }
                    break;
                case MessagePriorities.low:
                    foreach (KeyValuePair<int, Queue<Message>> conn in m_lowPrioQueue)
                    {
                        conn.Value.Enqueue(msg);
                    }
                    break;
            }
        }
    }
}