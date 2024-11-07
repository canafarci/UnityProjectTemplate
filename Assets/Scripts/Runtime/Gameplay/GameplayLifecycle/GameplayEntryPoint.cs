using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Lofelt.NiceVibrations;
using ProjectTemplate.Runtime.CrossScene.LoadingScreen.Signals;
using ProjectTemplate.Runtime.CrossScene.Signals;
using VContainer;
using VContainer.Unity;

using ProjectTemplate.Runtime.Gameplay.Enums;
using ProjectTemplate.Runtime.Gameplay.Signals;
using ProjectTemplate.Runtime.Infrastructure.ApplicationState;
using ProjectTemplate.Runtime.Infrastructure.ApplicationState.Signals;
using ProjectTemplate.Runtime.Infrastructure.Data;
using ProjectTemplate.Runtime.Infrastructure.Signals;
using UnityEngine;

namespace ProjectTemplate.Runtime.Gameplay.GameplayLifecycle
{
	public class GameplayEntryPoint : IInitializable, IStartable, IDisposable
	{
		[Inject] private SignalBus _signalBus;
		[Inject] ApplicationSettings _applicationSettings;
		
		public void Initialize()
		{
			if (_applicationSettings.ShowLoadingScreen)
				_signalBus.Subscribe<LoadingFinishedSignal>(OnLoadingFinishedSignal);
		}

		private void OnLoadingFinishedSignal(LoadingFinishedSignal signal)
		{
			EnterGameplay();
		}

		public void Start()
		{
			if (!_applicationSettings.ShowLoadingScreen)
				EnterGameplay();
		}

		private void EnterGameplay()
		{
			_signalBus.Fire(new ChangeAppStateSignal(AppStateID.Gameplay));
			_signalBus.Fire(new ChangeGameStateSignal(GameState.Initializing));
			
			InitializeGameplay();
			
			_signalBus.Fire(new ChangeGameStateSignal(GameState.Playing));
		}

		private void InitializeGameplay()
		{
			Debug.Log("Initialized Gameplay Scene");
		}
		
		public void Dispose()
		{
			if (_applicationSettings.ShowLoadingScreen)
				_signalBus.Unsubscribe<LoadingFinishedSignal>(OnLoadingFinishedSignal);
		}
	}
}