using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Networking.Services;

namespace Networking.Behaviours
{
    public class PlayerBehaviour : MonoBehaviour
    {
        [Inject] private IPlayerPositionBehaviour m_playerPositionBehaviour;
        [Inject] public IPlayerSpellController m_playerSpellController;

        public class Factory : PlaceholderFactory<PlayerBehaviour> { }
    }
}
