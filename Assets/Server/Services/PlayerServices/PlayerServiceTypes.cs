using System.Collections;
using System.Collections.Generic;
using Khan_Shared.Simulation;
using Networking.Behaviours;
using UnityEngine;

namespace Networking.Services
{
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
