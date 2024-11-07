using System;
using Cysharp.Threading.Tasks;
using Lofelt.NiceVibrations;
using ProjectTemplate.Runtime.CrossScene.Signals;
using VContainer;
using VContainer.Unity;

using ProjectTemplate.Runtime.Gameplay.Enums;
using ProjectTemplate.Runtime.Gameplay.Signals;
using ProjectTemplate.Runtime.Infrastructure.ApplicationState;
using ProjectTemplate.Runtime.Infrastructure.ApplicationState.Signals;
using ProjectTemplate.Runtime.Infrastructure.Data;
using ProjectTemplate.Runtime.Infrastructure.Signals;

namespace ProjectTemplate.Runtime.Gameplay.GameplayLifecycle
{
	public class GameplayEntryPoint : IStartable
	{
		[Inject] private SignalBus _signalBus;
		[Inject] ApplicationSettings _applicationSettings;
		
		public async void Start()
		{
			_signalBus.Fire(new ChangeAppStateSignal(AppStateID.Gameplay));
			_signalBus.Fire(new ChangeGameStateSignal(GameState.Initializing));
			
			InitializeGameplay();

			if (_applicationSettings.ShowLoadingScreen)
				await UniTask.Delay(TimeSpan.FromSeconds(_applicationSettings.LoadingScreenFadeDuration));
			
			_signalBus.Fire(new ChangeGameStateSignal(GameState.Playing));
		}

		private void InitializeGameplay()
		{

		}
	}
}