using UnityEngine;
using System.Collections;
using Zenject;

namespace Networking.Behaviours
{
    public class TestBehaviour : MonoBehaviour
    {
        [Inject] private readonly Spell_Fire_FlameTower.Factory factory;
        [Inject] private Transform g_root;

        private void Update()
        {
            if (Input.GetKeyDown("c"))
            {
                Spell_Fire_FlameTower flameTower = factory.Create();
                flameTower.gameObject.transform.SetParent(g_root);
                flameTower.gameObject.transform.position = Vector3.zero;
            }
        }
    }
}