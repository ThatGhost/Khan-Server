using System.Collections;
using System.Collections.Generic;
using Unity.Networking.Transport;
using UnityEngine;

using Khan_Shared.Networking;
using Networking.EntryPoints;

using Zenject;

namespace Networking.Core
{
    public class MessageQueue: IMessageQueue
    {
        [Inject] private readonly ICoder m_Coder;
        [Inject] private readonly IEntryPointRegistry m_entryPointRegistry;

        private Dictionary<int, Queue<Message>> out_queue;
        private Dictionary<int, Queue<Message>> in_queue;
        private Dictionary<int, Queue<Message>> relaible_queue;

        private Dictionary<MessageTypes, MessageFunctionPair.OnEntry> m_entryFunctions;

        public void Init()
        {
            out_queue = new Dictionary<int, Queue<Message>>();
            in_queue = new Dictionary<int, Queue<Message>>();
            relaible_queue = new Dictionary<int, Queue<Message>>();

            m_entryFunctions = new Dictionary<MessageTypes, MessageFunctionPair.OnEntry>();
            m_entryPointRegistry.Init();

            foreach (var function in m_entryPointRegistry.MessagePairs)
            {
                m_entryFunctions.Add(function.MessageType, function.entryFunction);
            }
        }

        public int OutQueueSize(int connection)
        {
            if (!out_queue.ContainsKey(connection))
                return 0;
            return out_queue[connection].Count;
        }

        public int InQueueSize(int connection)
        {
            if (!in_queue.ContainsKey(connection))
                return 0;
            return in_queue[connection].Count;
        }

        public int RelaibleQueueSize(int connection)
        {
            if (!relaible_queue.ContainsKey(connection))
                return 0;
            return relaible_queue[connection].Count;
        }

        public void PublishMessage(Message message, int connection)
        {
            if (!out_queue.ContainsKey(connection))
                out_queue.Add(connection, new Queue<Message>());
            if (!relaible_queue.ContainsKey(connection))
                relaible_queue.Add(connection, new Queue<Message>());

            if (message.IsReliable) relaible_queue[connection].Enqueue(message);
            else out_queue[connection].Enqueue(message);
        }

        public void PublishMessages(Message[] messages, int connection)
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

        public void DequeueMessages(ref DataStreamWriter stream, int connection)
        {
            if (!out_queue.ContainsKey(connection))
                out_queue.Add(connection, new Queue<Message>());

            while (out_queue[connection].Count > 0)
            {
                Message msg = out_queue[connection].Dequeue();
                m_Coder.EncodeRawMessage(ref stream, msg);
            }
        }

        public void ReadMessage(ref DataStreamReader stream, int connection)
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
                m_Coder.EncodeRawMessage(ref stream, msg);
            }
        }
    }
}