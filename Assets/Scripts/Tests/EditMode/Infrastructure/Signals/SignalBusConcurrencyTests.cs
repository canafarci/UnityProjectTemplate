using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using ProjectTemplate.Runtime.Infrastructure.Signals;

namespace ProjectTemplate.Tests.EditMode.Infrastructure.Signals
{
    public class SignalBusConcurrencyTests
    {
        private Type _signalSubscriptionType;
        private object _signalSubscription;
        private MethodInfo _addMethod;
        private MethodInfo _removeMethod;
        private MethodInfo _invokeMethod;
        private MethodInfo _hasHandlersMethod;

        [SetUp]
        public void SetUp()
        {
            // Retrieve the SignalSubscription type from the SignalBus assembly
            var signalBusType = typeof(SignalBus);
            _signalSubscriptionType = signalBusType.GetNestedType("SignalSubscription", BindingFlags.NonPublic);

            // Create an instance of SignalSubscription for a sample signal type
            _signalSubscription = Activator.CreateInstance(_signalSubscriptionType, typeof(TestSignal));

            // Get the Add, Remove, Invoke, and HasHandlers methods using reflection
            _addMethod = _signalSubscriptionType.GetMethod("Add", BindingFlags.Public | BindingFlags.Instance);
            _removeMethod = _signalSubscriptionType.GetMethod("Remove", BindingFlags.Public | BindingFlags.Instance);
            _invokeMethod = _signalSubscriptionType.GetMethod("Invoke", BindingFlags.Public | BindingFlags.Instance);
            _hasHandlersMethod = _signalSubscriptionType.GetMethod("HasHandlers", BindingFlags.Public | BindingFlags.Instance);
        }

        [Test]
        public void ConcurrentSubscriptions_Should_Not_Cause_Race_Conditions()
        {
            // Arrange
            int numThreads = 100;
            Task[] tasks = new Task[numThreads];
            Action<TestSignal> handler = signal => { };

            // Act
            for (int i = 0; i < numThreads; i++)
            {
                tasks[i] = Task.Run(() => _addMethod.Invoke(_signalSubscription, new object[] { handler }));
            }

            Task.WaitAll(tasks);

            // Assert
            bool hasHandlers = (bool)_hasHandlersMethod.Invoke(_signalSubscription, null);
            Assert.IsTrue(hasHandlers, "Handlers should be added without race conditions.");
        }

        [Test]
        public void ConcurrentUnsubscriptions_Should_Not_Cause_Race_Conditions()
        {
            // Arrange
            int numThreads = 100;
            Task[] tasks = new Task[numThreads];
            Action<TestSignal> handler = signal => { };

            // Add handler once
            _addMethod.Invoke(_signalSubscription, new object[] { handler });

            // Act
            for (int i = 0; i < numThreads; i++)
            {
                tasks[i] = Task.Run(() => _removeMethod.Invoke(_signalSubscription, new object[] { handler }));
            }

            Task.WaitAll(tasks);

            // Assert
            bool hasHandlers = (bool)_hasHandlersMethod.Invoke(_signalSubscription, null);
            Assert.IsFalse(hasHandlers, "Handlers should be removed without race conditions.");
        }

        [Test]
        public void ConcurrentInvocations_Should_Not_Cause_Race_Conditions()
        {
            // Arrange
            int numThreads = 100;
            Task[] tasks = new Task[numThreads];
            int invocationCount = 0;
            Action<TestSignal> handler = signal => Interlocked.Increment(ref invocationCount);
            _addMethod.Invoke(_signalSubscription, new object[] { handler });

            // Act
            for (int i = 0; i < numThreads; i++)
            {
                tasks[i] = Task.Run(() => _invokeMethod.Invoke(_signalSubscription, new object[] { new TestSignal() }));
            }

            Task.WaitAll(tasks);

            // Assert
            Assert.AreEqual(numThreads, invocationCount, "Handlers should be invoked exactly once per thread.");
        }

        private class TestSignal { }
    }
}