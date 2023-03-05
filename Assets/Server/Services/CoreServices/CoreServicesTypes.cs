using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Khan_Shared.Networking;

namespace Networking.Services
{
    public interface IMessagePublisher
    {
        public void PublishMessage(Message msg, int connection);
        public void PublishGlobalMessage(Message msg);
    }
}