using System.Collections;
using System.Collections.Generic;
using Networking.Services;
using UnityEngine;

public class MonoHelpers : MonoBehaviour, IMonoHelper
{
    public void StartCourotine(IEnumerator enumerator)
    {
        StartCoroutine(enumerator);
    }
}
