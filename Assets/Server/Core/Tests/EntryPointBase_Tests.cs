using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

using Khan_Shared.Networking;
using Server.EntryPoints;

public class EntryPointBase_Tests
{
    [Test]
    public void EntryPointBase_TestsSimplePasses()
    {
        // Arrange
        TestEntryPoint testClass = new TestEntryPoint();

        // Act
        testClass.Messages[0].entryFunction.Invoke(new object[0], 1);

        // Assert
        Assert.IsTrue(testClass.HasInitialized);

        MessageFunctionPair[] pairs = testClass.Messages;
        Assert.AreEqual(1, pairs.Length);
        Assert.AreEqual(pairs[0].MessageType, MessageTypes.Default);
        Assert.IsTrue(testClass.HasTriggered);
    }

    private class TestEntryPoint : EntryPointBase
    {
        public bool HasTriggered = false;
        public bool HasInitialized = false;

        public TestEntryPoint()
        {
            HasInitialized = true;
            p_messages = new MessageFunctionPair[]
            {
                new MessageFunctionPair(MessageTypes.Default, testFunction)
            };
        }

        private void testFunction(object[] data, int conn)
        {
            HasTriggered = true;
        }
    }
}