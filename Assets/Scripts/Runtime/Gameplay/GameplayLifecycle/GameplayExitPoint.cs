using ProjectTemplate.Runtime.CrossScene.Data;
using ProjectTemplate.Runtime.CrossScene.Signals;
using ProjectTemplate.Runtime.Gameplay.GameplayLifecycle.GameStates;
using ProjectTemplate.Runtime.Gameplay.Signals;
using ProjectTemplate.Runtime.Infrastructure.Data;
using ProjectTemplate.Runtime.Infrastructure.Templates;

namespace ProjectTemplate.Runtime.Gameplay.GameplayLifecycle
{
	public class GameplayExitPoint : SignalListener
	{
		private readonly ApplicationSettings _applicationSettings;
		private readonly IGameStateModel _gameStateModel;
		private readonly IGameplayPersistentData _gameplayPersistentData;

		public GameplayExitPoint(ApplicationSettings applicationSettings,
		                         IGameStateModel gameStateModel,
		                         IGameplayPersistentData gameplayPersistentData)
		{
			_applicationSettings = applicationSettings;
			_gameStateModel = gameStateModel;
			_gameplayPersistentData = gameplayPersistentData;
		}		
		
		protected override void SubscribeToEvents()
		{
			_signalBus.Subscribe<ExitGameplayLevelSignal>(OnExitGameplayLevelSignal);
		}

		private void OnExitGameplayLevelSignal(ExitGameplayLevelSignal signal)
		{
			int targetSceneIndex = GetNextLevelIndex();
			
			_signalBus.Fire(new LoadSceneSignal(targetSceneIndex));
		}
		
		private int GetNextLevelIndex()
		{
			if (_gameStateModel.isGameWon)
			{
				_gameplayPersistentData.IncreaseTargetSceneIndex();
			}

			int sceneIndex = _applicationSettings.HasMainMenu ?
				                                            _applicationSettings.MainMenuSceneIndex :
				                                            _gameplayPersistentData.levelToLoadIndex;
			return sceneIndex;
		}

		protected override void UnsubscribeFromEvents()
		{
			_signalBus.Unsubscribe<ExitGameplayLevelSignal>(OnExitGameplayLevelSignal);
		}
	}
}