using System;

using ProjectTemplate.Infrastructure.Signals;

using VContainer;
using VContainer.Unity;

namespace ProjectTemplate.Infrastructure.Templates
{
	public abstract class SignalListener : IInitializable, IDisposable
	{
		[Inject] protected SignalBus _signalBus;
		
		public virtual void Initialize()
		{
			SubscribeToSignals();
		}
		
		protected abstract void SubscribeToSignals();
		protected abstract void UnsubscribeToSignals();
		
		public virtual void Dispose()
		{
			UnsubscribeToSignals();
		}
	}
}