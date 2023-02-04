using System.Collections.Generic;
using System;
using Khan_Shared.Networking;
using Unity.Networking.Transport;
using Unity.Collections;
using Networking.Shared;

namespace Networking.Core
{
    public class Coder: ICoder
    {
        public Message[] DecodeRawMessage(ref DataStreamReader stream)
        {
            List<Message> messages = new List<Message>();

            while (stream.GetBytesRead() < stream.Length)
            {
                Message msg = new Message();
                msg.MessageType = (MessageTypes)stream.ReadUShort();
                MessagePair registeredPair = NetworkingCofigurations.getMessagePair(msg.MessageType);
                foreach (var type in registeredPair.Types)
                {
                    ReadTypes(ref stream, ref msg, type);
                }
                messages.Add(msg);
            }
            return messages.ToArray();
        }

        private void ReadTypes(ref DataStreamReader stream, ref Message msg, Type type)
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

        public void EncodeRawMessage(ref DataStreamWriter stream, Message message)
        {
            stream.WriteUShort((UInt16)(short)message.MessageType);
            MessagePair registeredPair = NetworkingCofigurations.getMessagePair(message.MessageType);
            for (int i = 0; i < registeredPair.Types.Length; i++)
            {
                writeTypes(ref stream, message.Data[i], registeredPair.Types[i]);
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