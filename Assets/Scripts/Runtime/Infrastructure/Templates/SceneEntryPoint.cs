using System;
using ProjectTemplate.Runtime.CrossScene.Signals;
using ProjectTemplate.Runtime.Infrastructure.Data;
using ProjectTemplate.Runtime.Infrastructure.Signals;
using VContainer;
using VContainer.Unity;

namespace ProjectTemplate.Runtime.Infrastructure.Templates
{
	public abstract class SceneEntryPoint : IInitializable, IStartable, IDisposable
	{
		[Inject] protected SignalBus _signalBus;
		[Inject] private ApplicationSettings _applicationSettings;

		public void Initialize()
		{
			if (_applicationSettings.ShowLoadingScreen)
				_signalBus.Subscribe<LoadingFinishedSignal>(OnLoadingFinishedSignal);
		}
		
		public void Start()
		{
			if (!_applicationSettings.ShowLoadingScreen)
				EnterScene();
		}
		
		private void OnLoadingFinishedSignal(LoadingFinishedSignal signal) => EnterScene();
		
		public void Dispose()
		{
			if (_applicationSettings.ShowLoadingScreen)
				_signalBus.Unsubscribe<LoadingFinishedSignal>(OnLoadingFinishedSignal);
		}
		
		protected abstract void EnterScene();
	}
}