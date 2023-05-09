using System;
using System.Collections;
using System.Collections.Generic;
using Networking.Services;
using UnityEngine;
using Zenject;

namespace Networking.Behaviours
{
    public class SpellUtil_BasicTimer : ISpellUtil_BasicTimer
    {
        public event Action onStartUp;
        public event Action onFireStart;
        public event Action onFireEnd;
        public event Action onEnd;

        [Inject] private IMonoHelper m_monoHelpers;

        public void start(float startUpTime, float fireTime, float endTime)
        {
            m_monoHelpers.StartCoroutine(startUp(startUpTime, fireTime, endTime));
        }

        private IEnumerator startUp(float startUpTime, float fireTime, float endTime)
        {
            yield return new WaitForSecondsRealtime(startUpTime);
            if (onStartUp != null) onStartUp.Invoke();
            m_monoHelpers.StartCoroutine(fire(fireTime, endTime));
        }

        private IEnumerator fire(float fireTime, float endTime)
        {
            if (onFireStart != null) onFireStart.Invoke();
            yield return new WaitForSecondsRealtime(fireTime);
            if (onFireEnd != null) onFireEnd.Invoke();
            m_monoHelpers.StartCoroutine(fire(fireTime, endTime));
        }

        private IEnumerator end(float endTime)
        {
            yield return new WaitForSecondsRealtime(endTime);
            if (onEnd != null) onEnd.Invoke();
        }
    }
}
