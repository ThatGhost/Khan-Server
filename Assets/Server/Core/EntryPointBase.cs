using System;

namespace Networking.EntryPoints
{
    public class EntryPointBase : IEntryPoint
    {
        protected MessageFunctionPair[] p_messages;

        public MessageFunctionPair[] Messages
        {
            get
            {
                return p_messages;
            }
        }
    }
}