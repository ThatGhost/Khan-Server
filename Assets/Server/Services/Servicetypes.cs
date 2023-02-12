using System;
using Khan_Shared.Networking;

namespace Networking.Services
{
    public interface IFooService
    {
        public void Foo(uint data, byte[] other, int conn);
    }

    public interface IMessagePublisher
    {
        public void PublishMessage(Message msg, int connection);
        public void PublishGlobalMessage(Message msg);
    }
}