using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Khan_Shared.Networking;

namespace Networking.Shared
{
    public class Message
    {
        private MessageTypes m_messageType;
        private List<object> m_data = new List<object>();

        public Message()
        {

        }

        public Message(MessageTypes type)
        {
            m_messageType = type;
        }

        public Message(MessageTypes type, List<object> data)
        {
            m_messageType = type;
            m_data = data;
        }

        public Message(MessageTypes type, object[] data)
        {
            m_messageType = type;
            m_data.AddRange(data);
        }

        public MessageTypes MessageType
        {
            get
            {
                return m_messageType;
            }
            set
            {
                m_messageType = value;
            }
        }

        public List<object> Data
        {
            get
            {
                return m_data;
            }
        }

        public void AddData(object data)
        {
            m_data.Add(data);
        }

        public int Lenght
        {
            get
            {
                return m_data.Count;
            }
        }
    }
}
