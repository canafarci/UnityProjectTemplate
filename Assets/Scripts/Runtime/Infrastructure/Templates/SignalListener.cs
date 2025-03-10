using System;
using ProjectTemplate.Runtime.Infrastructure.Signals;
using VContainer;
using VContainer.Unity;

namespace ProjectTemplate.Runtime.Infrastructure.Templates
{
public abstract class SignalListener : IInitializable, IDisposable
{
	[Inject] protected SignalBus _signalBus;
	
	public virtual void Initialize()
	{
		SubscribeToEvents();
	}
	
	protected abstract void SubscribeToEvents();
	protected abstract void UnsubscribeFromEvents();
	
	public virtual void Dispose()
	{
		UnsubscribeFromEvents();
	}
}
}