using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Khan_Shared.Networking;
using Khan_Shared.Magic;

namespace Networking.Services
{
    public interface IMessagePublisher
    {
        public void PublishMessage(Message msg, int connection);
        public void PublishGlobalMessage(Message msg);
    }

    public interface ISpellInitializer
    {
        public void InitializeSpells(int connection);
    }
}