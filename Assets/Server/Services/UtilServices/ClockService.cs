using System.Collections;
using System.Collections.Generic;
using Server.Services;
using UnityEngine;
using Zenject;
using static Server.Services.IClockService;

namespace Server.Services
{
    public class ClockService : IClockService
    {
        [Inject] private readonly IMonoHelper m_monoHelper;
        private OnClockTick m_onClockTick;
        public OnClockTick onClockTick
        {
            get
            {
                return m_onClockTick;
            }
            set
            {
                m_onClockTick += value;
            }
        }

        private Coroutine m_coroutine;

        public void StartClock(float interval)
        {
            m_coroutine = m_monoHelper.StartCoroutine(clock(interval));
        }

        public void StopClock()
        {
            m_monoHelper.StopCoroutine(m_coroutine);
        }

        private IEnumerator clock(float interval)
        {
            yield return new WaitForSeconds(interval);
            m_onClockTick.Invoke();
            m_coroutine = m_monoHelper.StartCoroutine(clock(interval));
        }
    }
}
