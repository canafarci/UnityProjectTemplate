using ProjectTemplate.Runtime.CrossScene.Data;
using ProjectTemplate.Runtime.CrossScene.Signals;
using ProjectTemplate.Runtime.Gameplay.GameplayLifecycle.GameStates;
using ProjectTemplate.Runtime.Gameplay.Signals;
using ProjectTemplate.Runtime.Infrastructure.Templates;

namespace ProjectTemplate.Runtime.Gameplay.GameplayLifecycle
{
	public class GameplayExitPoint : SignalListener
	{
		private readonly IGameStateModel _gameStateModel;
		private readonly IGameplayPersistentData _gameplayPersistentData;

		public GameplayExitPoint(IGameStateModel gameStateModel, IGameplayPersistentData gameplayPersistentData)
		{
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

			return _gameplayPersistentData.levelToLoadIndex;
		}

		protected override void UnsubscribeFromEvents()
		{
			_signalBus.Unsubscribe<ExitGameplayLevelSignal>(OnExitGameplayLevelSignal);
		}
	}
}