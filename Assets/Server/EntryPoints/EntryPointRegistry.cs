using System;
using System.Collections;
using System.Collections.Generic;
using Zenject;

namespace Networking.EntryPoints
{
    public class EntryPointRegistry : IEntryPointRegistry
    {
        // Add injects
        [Inject] private readonly ITestEntryPoint m_TestEntryPoint;

        // Then add to m_FunctionPairs
        public void Init()
        {
            m_FunctionPairs.AddRange(m_TestEntryPoint.Messages);
        }




        // Dont touch
        private List<MessageFunctionPair> m_FunctionPairs = new List<MessageFunctionPair>();
        public MessageFunctionPair[] MessagePairs
        {
            get
            {
                return m_FunctionPairs.ToArray();
            }
        }
    }
}