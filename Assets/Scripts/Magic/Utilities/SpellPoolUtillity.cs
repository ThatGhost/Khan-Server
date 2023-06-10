using System.Collections;
using System.Collections.Generic;
using Khan_Shared.Magic;
using Server.Magic;
using UnityEngine;
using Zenject;

namespace Server.Magic
{
    public class SpellPoolUtillity : ISpellPoolUtillity
    {
        [Inject] private readonly DiContainer m_container;

        private List<PrefabBuilder> m_pool = new List<PrefabBuilder>();
        private GameObject m_prefab;

        public void setup(GameObject prefab)
        {
            m_prefab = prefab;
        }

        public void release(GameObject gameObject)
        {
            foreach (var poolObject in m_pool)
            {
                if (gameObject == poolObject.gameObject) poolObject.gameObject.SetActive(false);
            }
        }

        public T request<T>(Spell spell) where T : PrefabBuilder
        {
            foreach (PrefabBuilder poolObject in m_pool)
            {
                if (poolObject.gameObject.activeSelf == false)
                {
                    poolObject.gameObject.SetActive(true);
                    return (T)poolObject;
                }
            }
            return instantiate<T>(spell);
        }

        private T instantiate<T>(Spell spell) where T : PrefabBuilder
        {
            PrefabBuilder newPoolObject = m_container.InstantiatePrefabForComponent<T>(m_prefab);
            m_pool.Add(newPoolObject);
            newPoolObject.build(spell);
            newPoolObject.onDestruction += onDestructionOfPrefab;

            return (T)newPoolObject;
        }

        private void onDestructionOfPrefab(PrefabBuilder obj)
        {
            release(obj.gameObject);
        }
    }
}
