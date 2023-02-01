using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

using Networking.Core;
using Unity.Networking.Transport;
using Networking.Shared;
using Unity.Collections;
using Khan_Shared.Networking;

public class Coder_Tests
{
    private Coder m_coder = new Coder();

    [Test]
    public void Coder_TestsRead()
    {
        // Arrage
        NativeArray<byte> data = new NativeArray<byte>(new byte[]
        {
            (byte)MessageTypes.Default,
            (byte)0,
            (byte)2,
            (byte)0,
            (byte)0,
            (byte)0,
        }, Allocator.Temp);
        DataStreamReader reader = new DataStreamReader(data);

        // Act
        Message[] response = m_coder.decodeRawMessages(ref reader);

        // Assert
        Assert.AreEqual(MessageTypes.Default, response[0].MessageType);
        Assert.IsFalse(reader.HasFailedReads);
    }

    [Test]
    public void Coder_TestsWrite()
    {
        // Arrage
        DataStreamWriter writer = new DataStreamWriter(10, Allocator.Temp);
        Message msg = new Message(MessageTypes.Default,new object[]{5});

        // Act
        m_coder.encodeToRawMessage(ref writer, msg);

        // Assert
        Assert.AreEqual(6, writer.Length);
        Assert.IsFalse(writer.HasFailedWrites);
    }
}
