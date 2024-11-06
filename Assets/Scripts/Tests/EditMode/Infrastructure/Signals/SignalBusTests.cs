using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using NUnit.Framework;
using ProjectTemplate.Runtime.Infrastructure.Signals;
using UnityEngine;
using UnityEngine.TestTools;

namespace ProjectTemplate.Tests.EditMode.Infrastructure.Signals
{
    public class SignalBusTests
    {
        private SignalBus _signalBus;

        [SetUp]
        public void SetUp()
        {
            // Define the signals that SignalBus should recognize
            List<Type> declaredSignals = new List<Type> { typeof(TestSignal) };
            _signalBus = new SignalBus(declaredSignals);
        }

        [Test]
        public void Subscribe_Should_Add_Handler_For_Declared_Signal()
        {
            // Arrange
            bool handlerCalled = false;
            Action<TestSignal> handler = signal => handlerCalled = true;

            // Act
            _signalBus.Subscribe(handler);
            _signalBus.Fire(new TestSignal());

            // Assert
            Assert.IsTrue(handlerCalled, "The handler should be called when the signal is fired.");
        }

        [Test]
        public void Unsubscribe_Should_Remove_Handler_For_Declared_Signal()
        {
            // Arrange
            bool handlerCalled = false;
            Action<TestSignal> handler = signal => handlerCalled = true;

            // Act
            _signalBus.Subscribe(handler);
            _signalBus.Unsubscribe(handler);
            
            var exception = Assert.Throws<InvalidOperationException>(() => _signalBus.Fire(new TestSignal()));
            Assert.AreEqual("No subscribers for signal 'TestSignal'.", exception.Message);

            // Assert
            Assert.IsFalse(handlerCalled, "The handler should not be called after being unsubscribed.");
        }

        [Test]
        public void Fire_Should_Invoke_All_Subscribed_Handlers_For_Signal()
        {
            // Arrange
            bool firstHandlerCalled = false;
            bool secondHandlerCalled = false;
            Action<TestSignal> firstHandler = signal => firstHandlerCalled = true;
            Action<TestSignal> secondHandler = signal => secondHandlerCalled = true;

            _signalBus.Subscribe(firstHandler);
            _signalBus.Subscribe(secondHandler);

            // Act
            _signalBus.Fire(new TestSignal());

            // Assert
            Assert.IsTrue(firstHandlerCalled, "The first handler should be called when the signal is fired.");
            Assert.IsTrue(secondHandlerCalled, "The second handler should be called when the signal is fired.");
        }

        [Test]
        public void Fire_Should_Throw_Exception_If_Signal_Not_Declared()
        {
            // Arrange
            var undeclaredSignal = new UndeclaredSignal();

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => _signalBus.Fire(undeclaredSignal));
            Assert.AreEqual("Signal 'UndeclaredSignal' has not been declared. Please declare it during container setup.", exception.Message);
        }

        [Test]
        public void Subscribe_Should_Throw_Exception_If_Signal_Not_Declared()
        {
            // Arrange
            Action<UndeclaredSignal> handler = signal => { };

            // Act & Assert
            InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() => _signalBus.Subscribe(handler));
            Assert.AreEqual("Signal 'UndeclaredSignal' has not been declared. Please declare it during container setup.", exception.Message);
        }

        [Test]
        public void Fire_Should_Throw_Exception_If_No_Handlers_Are_Subscribed()
        {
            // Arrange
            var testSignal = new TestSignal();

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => _signalBus.Fire(testSignal));
            Assert.AreEqual("No subscribers for signal 'TestSignal'.", exception.Message);
        }

        [Test]
        public void Fire_Should_Log_Error_If_Handler_Throws_Exception()
        {
            // Arrange
            Action<TestSignal> handler = signal => throw new Exception("Handler error");
            _signalBus.Subscribe(handler);

            // Use Unity's log capture to verify error logging
            LogAssert.Expect(LogType.Error, new Regex("Handler error"));
            // Act
            _signalBus.Fire(new TestSignal());

            // Assert
            LogAssert.NoUnexpectedReceived();
        }
        
        [Test]
        public void Fire_Should_Log_Error_If_Handler_Throws_Direct_Exception()
        {
            // Arrange
            Action<TestSignal> handler = null;
            _signalBus.Subscribe(handler);

            // Expect the direct exception to be logged
            LogAssert.Expect(LogType.Error, new Regex("Object reference not set to an instance of an object"));

            // Act
            _signalBus.Fire(new TestSignal());

            // Assert that there are no unexpected log entries
            LogAssert.NoUnexpectedReceived();
        }

        // Test signal classes for testing purposes
        private struct TestSignal { }
        private struct UndeclaredSignal { }
    }
}