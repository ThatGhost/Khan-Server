using System.Collections.Generic;
using System;
using Khan_Shared.Networking;
using Unity.Networking.Transport;
using Unity.Collections;

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

        private void ReadTypes(ref DataStreamReader stream, ref Message msg, DataType type)
        {
            switch (type)
            {
                case DataType.Uint:
                    msg.AddData(stream.ReadUInt());
                    break;
                case DataType.Int:
                    msg.AddData(stream.ReadInt());
                    break;
                case DataType.Ushort:
                    msg.AddData(stream.ReadUShort());
                    break;
                case DataType.Short:
                    msg.AddData(stream.ReadShort());
                    break;
                case DataType.Float:
                    msg.AddData(stream.ReadFloat());
                    break;
                case DataType.Bool:
                    msg.AddData(stream.ReadByte());
                    break;
                case DataType.Byte:
                    msg.AddData(stream.ReadByte());
                    break;
                case DataType.Varied:
                    ushort size = stream.ReadUShort();
                    List<byte> bytes = new List<byte>();
                    bytes.AddRange(BitConverter.GetBytes(size));
                    for (int i = 0; i < size; i++)
                    {
                        bytes.Add(stream.ReadByte());
                    }
                    msg.AddData(bytes.ToArray());
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

        private void writeTypes(ref DataStreamWriter writer, object obj, DataType type)
        {
            switch (type)
            {
                case DataType.Uint:
                    writer.WriteUInt((uint)obj);
                    break;
                case DataType.Int:
                    writer.WriteInt((int)obj);
                    break;
                case DataType.Ushort:
                    writer.WriteUShort((ushort)obj);
                    break;
                case DataType.Short:
                    writer.WriteShort((short)obj);
                    break;
                case DataType.Float:
                    writer.WriteFloat((float)obj);
                    break;
                case DataType.Bool:
                    writer.WriteByte(Convert.ToByte((bool)obj));
                    break;
                case DataType.Byte:
                    writer.WriteByte((byte)obj);
                    break;
                case DataType.Varied:
                    byte[] bytes = (byte[])obj;
                    if (bytes.Length == 0) throw new Exception("Lenght of varied datatype to short");
                    if (bytes.Length > 400) throw new Exception("Lenght of varied datatype to big");

                    ushort size = (ushort)bytes.Length;
                    writer.WriteUShort(size);
                    for (int i = 0; i < size; i++)
                    {
                        writer.WriteByte(bytes[i]);
                    }
                    break;
            }
        }
    }
}