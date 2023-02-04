using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Networking.Transport;
using Networking.Shared;
using Khan_Shared.Networking;

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
    public void InvokeMessages();
    public void DequeueMessages(ref DataStreamWriter stream, int connection);
    public void PublishMessage(Message message, int connection);
    public void ReadMessage(ref DataStreamReader stream, int connection);
}

public interface ICoder
{
    public Message[] DecodeRawMessage(ref DataStreamReader stream);
    public void EncodeRawMessage(ref DataStreamWriter stream, Message message);
}

