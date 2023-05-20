using System.Collections;
using System.Collections.Generic;
using Khan_Shared.Simulation;
using Khan_Shared.Magic;
using Networking.Behaviours;
using UnityEditor.MemoryProfiler;
using UnityEngine;

namespace Networking.Services
{
    public interface IPlayerInputService
    {
        public void ReceivePlayerInput(SInput[] input, int connection);
    }

    public interface IPlayersController
    {
        public void AddPlayer(PlayerBehaviour playerBehaviour, int connection);
        public PlayerRefrenceObject? getPlayer(int connection);
        public PlayerRefrenceObject? getPlayer(GameObject gameObject);
        public PlayerRefrenceObject[] getPlayers();
    }

    public interface IPlayerSpellController
    {
        public void receiveInput(PlayerRefrenceObject connection, SInput[] inputs);
        public void addSpell(int key, PlayerSpell playerSpell);
        public Spell[] getSpells();
    }

    public struct PlayerRefrenceObject
    {
        public int _connectionId;
        public GameObject _gameObject;
        public PlayerBehaviour _playerBehaviour;
        public PlayerPositionBehaviour _playerPositionBehaviour;
        public IPlayerSpellController _playerSpellController;
    }

    public struct PlayerSpell
    {
        public Spell spell;
        public int playerSpellId;
    }
}
