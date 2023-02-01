using System.Collections.Generic;
using System;
using Khan_Shared.Networking;
using Unity.Networking.Transport;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.Collections;
using System.Reflection;
using Networking.Shared;

using UnityEngine;

namespace Networking.Core
{
    public class Coder
    {
        public Message[] decodeRawMessages(ref DataStreamReader stream)
        {
            List<Message> messages = new List<Message>();

            while (stream.GetBytesRead() < stream.Length)
            {
                Message msg = new Message();
                msg.MessageType = (MessageTypes)stream.ReadUShort();
                MessagePair registeredPair = NetworkingCofigurations.getMessagePair(msg.MessageType);
                foreach (var type in registeredPair.Types)
                {
                    switch (type.ToString())
                    {
                        case "System.Int32":
                            msg.AddData(stream.ReadInt());
                            break;
                        case "System.UInt16":
                            msg.AddData(stream.ReadUShort());
                            break;
                        case "System.Single":
                            msg.AddData(stream.ReadFloat());
                            break;
                        case "System.Boolean":
                            msg.AddData(stream.ReadByte());
                            break;
                        case "System.Byte":
                            msg.AddData(stream.ReadByte());
                            break;
                    }
                }
                messages.Add(msg);
            }
            return messages.ToArray();
        }

        public void encodeToRawMessage(ref DataStreamWriter writer, Message msg)
        {
            writer.WriteUShort((UInt16)(short)msg.MessageType);
            MessagePair registeredPair = NetworkingCofigurations.getMessagePair(msg.MessageType);
            for (int i = 0; i < registeredPair.Types.Length; i++)
            {
                writeTypes(ref writer, msg.Data[i], registeredPair.Types[i]);
            }
        }

        private void writeTypes(ref DataStreamWriter writer, object obj, Type type)
        {
            switch (type.ToString())
            {
                case "System.Int32":
                    writer.WriteInt((int)obj);
                    break;
                case "System.UInt16":
                    writer.WriteUShort((ushort)obj);
                    break;
                case "System.Single":
                    writer.WriteFloat((float)obj);
                    break;
                case "System.Boolean":
                    writer.WriteByte(Convert.ToByte((bool)obj));
                    break;
                case "System.Byte":
                    writer.WriteByte((byte)obj);
                    break;
            }
        }
    }
}