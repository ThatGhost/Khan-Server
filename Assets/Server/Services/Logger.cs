using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Networking.Services
{
    public class Logger : ILogger
    {
        public void LogMessage(string message)
        {
            Debug.Log(message);
        }

        public void LogError(string message)
        {
            Debug.LogError(message);
        }
    }

}