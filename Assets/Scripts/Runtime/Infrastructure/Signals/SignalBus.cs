using System;
using System.Collections.Generic;
using System.Threading;
using ProjectTemplate.Runtime.Infrastructure.ApplicationState.Signals;
using UnityEngine;

namespace ProjectTemplate.Runtime.Infrastructure.Signals
{
    public sealed class SignalBus
    {
        private readonly Dictionary<Type, SignalSubscription> _subscriptions = new();

        public void DeclareSignal<TSignal>()
        {
            Type signalType = typeof(TSignal);
            if (!_subscriptions.ContainsKey(signalType))
            {
                _subscriptions[signalType] = new SignalSubscription(signalType);
            }
        }

        public void Subscribe<TSignal>(Action<TSignal> handler)
        {
            Type signalType = typeof(TSignal);
            if (!_subscriptions.TryGetValue(signalType, out SignalSubscription subscription))
            {
                // Declare the signal dynamically if it hasn't been declared yet
                DeclareSignal<TSignal>();
                subscription = _subscriptions[signalType];
            }

            subscription.Add(handler);
        }

        public void Unsubscribe<TSignal>(Action<TSignal> handler)
        {
            Type signalType = typeof(TSignal);

            if (_subscriptions.TryGetValue(signalType, out SignalSubscription subscription))
            {
                subscription.Remove(handler);
            }
        }

        public void Fire<TSignal>(TSignal signal)
        {
            Type signalType = typeof(TSignal);
            if (_subscriptions.TryGetValue(signalType, out SignalSubscription subscription))
            {
                if (subscription.HasHandlers())
                {
                    subscription.Invoke(signal);
                }
                else
                {
                    Debug.LogWarning($"No subscribers for signal '{signalType.Name}'.");
                }
            }
            else
            {
                // Optionally declare the signal if it hasn't been declared
                DeclareSignal<TSignal>();
                Debug.LogWarning($"Signal '{signalType.Name}' has been dynamically declared but has no subscribers.");
            }
        }

        private class SignalSubscription
        {
            private readonly List<Delegate> _handlers = new();
            private readonly ReaderWriterLockSlim _lock = new();
            private readonly Type _signalType;

            public SignalSubscription(Type signalType)
            {
                _signalType = signalType;
            }

            public void Add(Delegate handler)
            {
                _lock.EnterWriteLock();
                try
                {
                    _handlers.Add(handler);
                }
                finally
                {
                    _lock.ExitWriteLock();
                }
            }

            public void Remove(Delegate handler)
            {
                _lock.EnterWriteLock();
                try
                {
                    _handlers.Remove(handler);
                }
                finally
                {
                    _lock.ExitWriteLock();
                }
            }

            public void Invoke(object signal)
            {
                Delegate[] handlersCopy;
                _lock.EnterReadLock();
                try
                {
                    handlersCopy = _handlers.ToArray();
                }
                finally
                {
                    _lock.ExitReadLock();
                }

                foreach (Delegate handler in handlersCopy)
                {
                    try
                    {
                        handler.DynamicInvoke(signal);
                    }
                    catch (Exception ex) when (ex.InnerException != null)
                    {
                        Debug.LogError(ex.InnerException);
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError(ex);
                    }
                }
            }

            public bool HasHandlers()
            {
                _lock.EnterReadLock();
                try
                {
                    // Exclude App State Changed Signal to
                    //avoid warnings in bootstrap
                    if (_signalType == typeof(AppStateChangedSignal))
                        return true;

                    return _handlers.Count > 0;
                }
                finally
                {
                    _lock.ExitReadLock();
                }
            }
        }
    }
}
