using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Server.Services;

namespace Server.Behaviours
{
    public class PlayerBehaviour : MonoBehaviour
    {
        [Inject] private IPlayerPositionBehaviour m_playerPositionBehaviour;
        [Inject] public IPlayerSpellController m_playerSpellController;
        [Inject] public IPlayerVariableService m_playerVariableService;

        public class Factory : PlaceholderFactory<PlayerBehaviour> { }
    }
}
