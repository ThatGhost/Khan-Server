using System;
using System.Collections.Generic;
using Khan_Shared.Networking;

public abstract class EntryPointBase
{
    protected List<MessageFunctionPair> p_messages;

    public void Init()
    {
        p_messages = new List<MessageFunctionPair>();
        Initialize();
    }

    protected abstract void Initialize();


    public List<MessageFunctionPair> Messages
    {
        get
        {
            return p_messages;
        }
    }

    public struct MessageFunctionPair
    {
        private MessageTypes m_messageType;
        public delegate void OnEntry(object[] data, int connection);
        private OnEntry onEntry;

        public MessageFunctionPair(MessageTypes messageType, OnEntry entryFunction)
        {
            m_messageType = messageType;
            onEntry = entryFunction;
        }

        public MessageTypes MessageType
        {
            get
            {
                return m_messageType;
            }
        }

        public OnEntry entryFunction
        {
            get
            {
                return onEntry;
            }
        }
    }
}

