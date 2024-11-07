using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using ProjectTemplate.Runtime.Infrastructure.ApplicationState.Signals;
using UnityEngine;

namespace ProjectTemplate.Runtime.Infrastructure.Signals
{
	public sealed class SignalBus
	{
		private readonly Dictionary<Type, SignalSubscription> _subscriptions = new();

		public SignalBus(IEnumerable<Type> declaredSignals)
		{
			// Initialize subscriptions for declared signals
			foreach (Type signalType in declaredSignals)
			{
				_subscriptions[signalType] = new SignalSubscription(signalType);
			}
		}

		public void Subscribe<TSignal>(Action<TSignal> handler)
		{
			Type signalType = typeof(TSignal);
			if (!_subscriptions.TryGetValue(signalType, out SignalSubscription subscription))
			{
				throw new InvalidOperationException($"Signal '{signalType.Name}' has not been declared. Please declare it during container setup.");
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
					throw new InvalidOperationException($"No subscribers for signal '{signalType.Name}'.");
				}
			}
			else
			{
				throw new InvalidOperationException($"Signal '{signalType.Name}' has not been declared. Please declare it during container setup.");
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
					//Exclude App State Changed Signal
					//Because this signal is used by Loading Screen in Bootstrap scene
					//Which might not get injected if it gets deselected in App Settings
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