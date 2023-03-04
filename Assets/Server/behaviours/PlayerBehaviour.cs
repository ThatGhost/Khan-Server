using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Networking.Behaviours
{
    public class PlayerBehaviour : MonoBehaviour
    {
        [Inject] private IPlayerPositionBehaviour m_playerPositionBehaviour;

        public class Factory : PlaceholderFactory<PlayerBehaviour> { }
    }
}
