using System.Collections;
using System.Collections.Generic;
using Unity.Networking.Transport;
using UnityEngine;
using Khan_Shared.Networking;
using Networking.Shared;

namespace Networking.Core
{
    public static class MessageQueue
    {
        private static Dictionary<int, Queue<Message>> out_queue;
        private static Dictionary<int, Queue<Message>> in_queue;
        private static Coder m_Coder;
        private static Dictionary<MessageTypes, EntryPointBase.MessageFunctionPair.OnEntry> m_entryFunctions;

        public static void init()
        {
            out_queue = new Dictionary<int, Queue<Message>>();
            in_queue = new Dictionary<int, Queue<Message>>();
            m_Coder = new Coder();
            m_entryFunctions = new Dictionary<MessageTypes, EntryPointBase.MessageFunctionPair.OnEntry>();
            EntryPointRegistry registry = new EntryPointRegistry();
            foreach (var EntryPoint in registry.EntryPoints)
            {
                foreach (var function in EntryPoint.Messages)
                {
                    m_entryFunctions.Add(function.MessageType, function.entryFunction);
                }
            }
        }

        public static int outQueueSize(int connection)
        {
            if (!out_queue.ContainsKey(connection))
                return 0;
            return out_queue[connection].Count;
        }

        public static int inQueueSize(int connection)
        {
            if (!in_queue.ContainsKey(connection))
                return 0;
            return in_queue[connection].Count;
        }

        public static void publishMessage(Message message, int connection)
        {
            if (!out_queue.ContainsKey(connection))
                out_queue.Add(connection, new Queue<Message>());

            out_queue[connection].Enqueue(message);
        }

        public static void enterMessages()
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

        public static void writeMessages(ref DataStreamWriter writer, int connection)
        {
            if (!out_queue.ContainsKey(connection))
                out_queue.Add(connection, new Queue<Message>());

            while (out_queue[connection].Count > 0)
            {
                Message msg = out_queue[connection].Dequeue();
                m_Coder.encodeToRawMessage(ref writer, msg);
            }
        }

        public static void enterMessage(ref DataStreamReader stream, int connection)
        {
            if (!in_queue.ContainsKey(connection))
                in_queue.Add(connection, new Queue<Message>());

            Message[] messages = m_Coder.decodeRawMessages(ref stream);
            foreach (var msg in messages)
            {
                in_queue[connection].Enqueue(msg);
            }
        }
    }
}