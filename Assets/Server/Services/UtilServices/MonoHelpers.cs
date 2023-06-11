using System.Collections;
using UnityEngine;

namespace Networking.Services
{
    public class MonoHelpers : MonoBehaviour, IMonoHelper
    {
        public new Object Instantiate(Object o)
        {
            return MonoBehaviour.Instantiate(o);
        }
    }
}
