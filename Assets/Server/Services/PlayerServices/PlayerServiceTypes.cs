using System.Collections;
using System.Collections.Generic;
using Khan_Shared.Utils;
using Khan_Shared.Magic;
using Server.Behaviours;
using UnityEditor.MemoryProfiler;
using UnityEngine;

namespace Server.Services
{
    public interface IPlayerInputService
    {
        public void ReceivePlayerInput(SInput input, int connection);
    }

    public interface IPlayersController
    {
        public void AddPlayer(PlayerBehaviour playerBehaviour, int connection);
        public void removePlayer(int connection);
        public PlayerRefrenceObject? getPlayer(int connection);
        public PlayerRefrenceObject? getPlayer(GameObject gameObject);
        public PlayerRefrenceObject[] getPlayers();
    }

    public interface IPlayerSpellController
    {
        public void clientTriggerSpell(PlayerRefrenceObject connection, SInput input);
        public void addSpell(PlayerSpell playerSpell, int key);
        public Spell[] getSpells();
        public bool playerOwnsSpell(int playerSpellId);
        public void destructSpells();
    }

    public interface IPlayerVariableService
    {
        public void setup(int connectionId);
        public int Mana { get; }
        public int Health { get; }
        public int MaxMana { get; set; }
        public int MaxHealth { get; set; }
    }

    public interface IPlayersVariableService
    {
        public void routeHealth(GameObject[] playerObject, int amount);
        public void routeHealth(GameObject[] playerObject, int amount, int playerSpellId);
        public void routeMana(int playerSpellId, int amount);
        public void onDeath(int connectionId);
    }

    public interface IPlayerDeathService
    {

    }

    public struct PlayerRefrenceObject
    {
        public int _connectionId;
        public GameObject _gameObject;
        public PlayerBehaviour _playerBehaviour;
        public PlayerPositionBehaviour _playerPositionBehaviour;
        public IPlayerSpellController _playerSpellController;
        public IPlayerVariableService _playerVariableService;
    }

    public struct PlayerSpell
    {
        public Spell spell;
        public int playerSpellId;
    }
}
