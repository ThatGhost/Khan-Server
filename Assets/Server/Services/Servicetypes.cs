using System;
using System.Collections;
using Khan_Shared.Networking;
using Khan_Shared.Simulation;
using Networking.Behaviours;
using UnityEngine;

namespace Networking.Services
{
    public interface IFooService
    {
        public void Foo(uint data, byte[] other, int conn);
    }

    public interface IMonoHelper
    {
        public void StartCourotine(IEnumerator enumerator);
    }

    public interface IMessagePublisher
    {
        public void PublishMessage(Message msg, int connection);
        public void PublishGlobalMessage(Message msg);
    }

    public interface ILogger
    {
        public void LogMessage(string message);
        public void LogError(string message);
    }

    public interface IPlayerInputService
    {
        public void ReceivePlayerInput(SInput[] input, int connection);
    }

    public struct PlayerRefrenceObject
    {
        public int _connectionId;
        public GameObject _gameObject;
        public PlayerBehaviour _playerBehaviour;
        public PlayerPositionBehaviour _playerPositionBehaviour;
    }
}