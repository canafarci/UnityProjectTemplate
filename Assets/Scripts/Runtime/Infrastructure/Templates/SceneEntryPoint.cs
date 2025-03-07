using System;
using ProjectTemplate.Runtime.CrossScene.Signals;
using ProjectTemplate.Runtime.Infrastructure.Data;
using ProjectTemplate.Runtime.Infrastructure.Signals;
using VContainer;
using VContainer.Unity;

namespace ProjectTemplate.Runtime.Infrastructure.Templates
{
	public abstract class SceneEntryPoint : IStartable
	{
		[Inject] protected SignalBus _signalBus;

		public void Start()
		{
			EnterScene();
		}
		
		protected abstract void EnterScene();
	}
}