using System;
using ProjectTemplate.Runtime.CrossScene.Signals;
using ProjectTemplate.Runtime.Infrastructure.Data;
using ProjectTemplate.Runtime.Infrastructure.Signals;
using VContainer;
using VContainer.Unity;

namespace ProjectTemplate.Runtime.Infrastructure.Templates
{
	public abstract class SceneEntryPoint : IInitializable
	{
		[Inject] protected SignalBus _signalBus;
		[Inject] private ApplicationSettings _applicationSettings;

		public void Initialize()
		{
			EnterScene();
		}
		
		protected abstract void EnterScene();
	}
}