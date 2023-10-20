using System.Collections;
using System.Collections.Generic;
using Unity.Networking.Transport;

using Khan_Shared.Networking;
using Server.EntryPoints;
using Server.Behaviours;

using Zenject;
using ConnectionId = System.Int32;

namespace Server.Core
{
    public class MessageQueue: IMessageQueue
    {
        [Inject] private readonly ICoder m_Coder;
        [Inject] private readonly IEntryPointRegistry m_entryPointRegistry;
        [Inject] private readonly IDebugStatBehaviour m_debugStatBehaviour;

        private Dictionary<ConnectionId, Queue<Message>> out_queue;
        private Dictionary<ConnectionId, Queue<Message>> in_queue;
        private Dictionary<ConnectionId, Queue<Message>> relaible_queue;

        private Dictionary<MessageTypes, MessageFunctionPair.OnEntry> m_entryFunctions;

        public void Init()
        {
            out_queue = new Dictionary<ConnectionId, Queue<Message>>();
            in_queue = new Dictionary<ConnectionId, Queue<Message>>();
            relaible_queue = new Dictionary<ConnectionId, Queue<Message>>();

            m_entryFunctions = new Dictionary<MessageTypes, MessageFunctionPair.OnEntry>();
            m_entryPointRegistry.Init();

            foreach (var function in m_entryPointRegistry.MessagePairs)
            {
                m_entryFunctions.Add(function.MessageType, function.entryFunction);
            }
        }

        public int OutQueueSize(ConnectionId connection)
        {
            if (!out_queue.ContainsKey(connection))
                return 0;
            return out_queue[connection].Count;
        }

        public int InQueueSize(ConnectionId connection)
        {
            if (!in_queue.ContainsKey(connection))
                return 0;
            return in_queue[connection].Count;
        }

        public int RelaibleQueueSize(ConnectionId connection)
        {
            if (!relaible_queue.ContainsKey(connection))
                return 0;
            return relaible_queue[connection].Count;
        }

        public void PublishMessage(Message message, ConnectionId connection)
        {
            if (!out_queue.ContainsKey(connection))
                out_queue.Add(connection, new Queue<Message>());
            if (!relaible_queue.ContainsKey(connection))
                relaible_queue.Add(connection, new Queue<Message>());

            if (message.IsReliable) relaible_queue[connection].Enqueue(message);
            else out_queue[connection].Enqueue(message);
        }

        public void PublishMessages(Message[] messages, ConnectionId connection)
        {
            if (!out_queue.ContainsKey(connection))
                out_queue.Add(connection, new Queue<Message>());
            if (!relaible_queue.ContainsKey(connection))
                relaible_queue.Add(connection, new Queue<Message>());

            foreach (var message in messages)
            {
                if (message.IsReliable) relaible_queue[connection].Enqueue(message);
                else out_queue[connection].Enqueue(message);
            }
        }

        public void InvokeMessages()
        {
            foreach (var conn in in_queue)
            {
                while (conn.Value.Count > 0)
                {
                    Message msg = conn.Value.Dequeue();
                    m_entryFunctions[msg.MessageType].Invoke(msg.Data.ToArray(), conn.Key);
                }
            }
        }

        public void DequeueMessages(ref DataStreamWriter stream, ConnectionId connection)
        {
            if (!out_queue.ContainsKey(connection))
                out_queue.Add(connection, new Queue<Message>());

            while (OutQueueSize(connection) > 0)
            {
                Message msg = out_queue[connection].Dequeue();

                m_debugStatBehaviour.addMessagesOut(1);

                m_Coder.EncodeRawMessage(ref stream, msg);
            }
        }

        public void ReadMessage(ref DataStreamReader stream, ConnectionId connection)
        {
            if (!in_queue.ContainsKey(connection))
                in_queue.Add(connection, new Queue<Message>());

            Message[] messages = m_Coder.DecodeRawMessage(ref stream);
            foreach (var msg in messages)
            {
                in_queue[connection].Enqueue(msg);
            }
        }

        public void DequeuRelaibleMessages(ref DataStreamWriter stream, int connection)
        {
            if (!relaible_queue.ContainsKey(connection))
                relaible_queue.Add(connection, new Queue<Message>());

            while (relaible_queue[connection].Count > 0)
            {
                Message msg = relaible_queue[connection].Dequeue();

                m_debugStatBehaviour.addMessagesOut(1);

                m_Coder.EncodeRawMessage(ref stream, msg);
            }
        }
    }
}