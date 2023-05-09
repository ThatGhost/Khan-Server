using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Networking.Services
{
    public interface IFooService
    {
        public void Foo(uint data, byte[] other, int conn);
    }

    public interface IMonoHelper
    {
        public void StartCoroutine(IEnumerator enumerator);
    }

    public interface ILoggerService
    {
        public void LogMessage(string message);
        public void LogError(string message);
    }
}