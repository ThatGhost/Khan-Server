using System;
using System.Collections;
using System.Collections.Generic;
using Zenject;

namespace Server.EntryPoints
{
    public class EntryPointRegistry : IEntryPointRegistry
    {
        // Add injects
        //[Inject] private readonly ITestEntryPoint m_TestEntryPoint;
        [Inject] private readonly IPlayerEntryPoint m_playerEntryPoint;

        // Then add to m_FunctionPairs
        public void Init()
        {
            //m_FunctionPairs.AddRange(m_TestEntryPoint.Messages);
            m_FunctionPairs.AddRange(m_playerEntryPoint.Messages);
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