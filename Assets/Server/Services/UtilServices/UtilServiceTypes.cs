using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Server.Services
{
    public interface IFooService
    {
        public void Foo(uint data, byte[] other, int conn);
    }

    public interface IMonoHelper
    {
        public Coroutine StartCoroutine(IEnumerator enumerator);
        public void StopCoroutine(Coroutine enumerator);
        public Object Instantiate(Object o);
        public void Destroy(Object obj, float t = 0.0f);
    }

    public interface ILoggerService
    {
        public void LogMessage(string message);
        public void LogError(string message);
    }

    public interface IClockService
    {
        public void StartClock(float interval);
        public void StopClock();

        public delegate void OnClockTick();
        public OnClockTick onClockTick { get; set; }
    }
}