using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Networking.Transport;
using Khan_Shared.Networking;

namespace Server.EntryPoints
{
    public interface IEntryPoint
    {
        public MessageFunctionPair[] Messages { get; }
    }

    public struct MessageFunctionPair
    {
        private MessageTypes m_messageType;
        public delegate void OnEntry(object[] data, int connection);
        private OnEntry onEntry;

        public MessageFunctionPair(MessageTypes messageType, OnEntry entryFunction)
        {
            m_messageType = messageType;
            onEntry = entryFunction;
        }

        public MessageTypes MessageType
        {
            get
            {
                return m_messageType;
            }
        }

        public OnEntry entryFunction
        {
            get
            {
                return onEntry;
            }
        }
    }
}

namespace Server.Core
{
    public interface IGameServer
    {
        public int Tick { get; }

        public delegate void OnServerTick(int tick);
        public static OnServerTick onServerTick;

        public delegate void OnClientConnect(int internalId);
        public static OnClientConnect onClientConnect;

        public delegate void OnClientDisconnect(int internalId);
        public static OnClientDisconnect onClientDisconnect;
    }

    public interface IMessageQueue
    {
        public void Init();

        public int OutQueueSize(int connection);
        public int InQueueSize(int connection);
        public int RelaibleQueueSize(int connection);

        public void InvokeMessages();
        public void DequeueMessages(ref DataStreamWriter stream, int connection);
        public void PublishMessage(Message message, int connection);
        public void PublishMessages(Message[] messages, int connection);
        public void ReadMessage(ref DataStreamReader stream, int connection);
        public void DequeuRelaibleMessages(ref DataStreamWriter stream, int connection);
    }

    public interface ICoder
    {
        public Message[] DecodeRawMessage(ref DataStreamReader stream);
        public void EncodeRawMessage(ref DataStreamWriter stream, Message message);
    }
}