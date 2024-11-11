using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using ProjectTemplate.Runtime.CrossScene.LoadingScreen.Signals;
using VContainer;
using VContainer.Unity;

using ProjectTemplate.Runtime.Gameplay.Enums;
using ProjectTemplate.Runtime.Gameplay.Signals;
using ProjectTemplate.Runtime.Infrastructure.ApplicationState;
using ProjectTemplate.Runtime.Infrastructure.ApplicationState.Signals;
using ProjectTemplate.Runtime.Infrastructure.Data;
using ProjectTemplate.Runtime.Infrastructure.MemoryPool;
using ProjectTemplate.Runtime.Infrastructure.Signals;
using UnityEngine;

namespace ProjectTemplate.Runtime.Gameplay.GameplayLifecycle
{
	public class GameplayEntryPoint : IInitializable, IStartable, IDisposable
	{
		[Inject] private SignalBus _signalBus;
		[Inject] private ApplicationSettings _applicationSettings;
		
		private async void EnterGameplay()
		{
			_signalBus.Fire(new ChangeAppStateSignal(AppStateID.Gameplay));
			_signalBus.Fire(new ChangeGameStateSignal(GameState.Initializing));
			
			InitializeGameplay();

			await UniTask.Delay(TimeSpan.FromSeconds(1f));
			
			_signalBus.Fire(new ChangeGameStateSignal(GameState.Playing));
		}

		private void InitializeGameplay()
		{
		}
		
		#region CONDITIONAL_INITIALIZATION_AND_SUBSCRIPTIONS

		public void Initialize()
		{
			if (_applicationSettings.ShowLoadingScreen)
				_signalBus.Subscribe<LoadingFinishedSignal>(OnLoadingFinishedSignal);
		}
		
		public void Start()
		{
			if (!_applicationSettings.ShowLoadingScreen)
				EnterGameplay();
		}
		
		private void OnLoadingFinishedSignal(LoadingFinishedSignal signal) => EnterGameplay();
		
		public void Dispose()
		{
			if (_applicationSettings.ShowLoadingScreen)
				_signalBus.Unsubscribe<LoadingFinishedSignal>(OnLoadingFinishedSignal);
		}

		#endregion


	}
}