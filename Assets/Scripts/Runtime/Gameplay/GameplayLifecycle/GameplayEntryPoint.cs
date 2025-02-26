using System;
using Cysharp.Threading.Tasks;
using ProjectTemplate.Runtime.CrossScene.Signals;
using ProjectTemplate.Runtime.Gameplay.Enums;
using ProjectTemplate.Runtime.Gameplay.GameplayLifecycle.Enums;
using ProjectTemplate.Runtime.Gameplay.Signals;
using ProjectTemplate.Runtime.Infrastructure.ApplicationState;
using ProjectTemplate.Runtime.Infrastructure.ApplicationState.Signals;
using ProjectTemplate.Runtime.Infrastructure.Templates;

namespace ProjectTemplate.Runtime.Gameplay.GameplayLifecycle
{
	public class GameplayEntryPoint : SceneEntryPoint
	{
		private readonly GameplayInitializer _gameplayInitializer;

		public GameplayEntryPoint(GameplayInitializer gameplayInitializer)
		{
			_gameplayInitializer = gameplayInitializer;
		}
		
		protected override async void EnterScene()
		{
			_signalBus.Fire(new ChangeAppStateSignal(AppStateID.Gameplay));
			_signalBus.Fire(new ChangeGameStateSignal(GameState.Initializing));
			
			await _gameplayInitializer.InitializeModules();
			
			_signalBus.Fire(new CloseLoadingScreenSignal());
			_signalBus.Fire(new ChangeGameStateSignal(GameState.Playing));
		}
	}
}