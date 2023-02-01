using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

using Networking.Shared;
using Khan_Shared.Networking;
using System.IO.Ports;

public class EntryPointBase_Tests
{
    [Test]
    public void EntryPointBase_TestsSimplePasses()
    {
        // Arrange
        TestEntryPoint testClass = new TestEntryPoint();

        // Act
        testClass.Init();
        testClass.Messages[0].entryFunction.Invoke(new object[0], 1);

        // Assert
        Assert.IsTrue(testClass.HasInitialized);

        List<EntryPointBase.MessageFunctionPair> pairs = testClass.Messages;
        Assert.AreEqual(1, pairs.Count);
        Assert.AreEqual(pairs[0].MessageType, MessageTypes.Default);
        Assert.IsTrue(testClass.HasTriggered);
    }

    private class TestEntryPoint : EntryPointBase
    {
        public bool HasTriggered = false;
        public bool HasInitialized = false;
        protected override void Initialize()
        {
            HasInitialized = true;

            // Message registering
            p_messages.Add(new MessageFunctionPair(MessageTypes.Default, testFunction));
        }

        private void testFunction(object[] data, int conn)
        {
            HasTriggered = true;
        }
    }
}