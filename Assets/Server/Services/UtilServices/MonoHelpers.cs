using System.Collections;
using System.Collections.Generic;
using Networking.Services;
using UnityEngine;

namespace Networking.Services
{
    public class MonoHelpers : MonoBehaviour, IMonoHelper
    {
        public new void StartCoroutine(IEnumerator enumerator)
        {
            base.StartCoroutine(enumerator);
        }

        public new Object Instantiate(Object o)
        {
            return MonoBehaviour.Instantiate(o);
        }
    }
}
