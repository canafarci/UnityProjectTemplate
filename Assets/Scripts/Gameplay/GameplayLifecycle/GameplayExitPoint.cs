using ProjectTemplate.CrossScene.Data;
using ProjectTemplate.CrossScene.Signals;
using VContainer;

using ProjectTemplate.Gameplay.GameplayLifecycle.GameStates;
using ProjectTemplate.Gameplay.Signals;
using ProjectTemplate.Infrastructure.Templates;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectTemplate.Gameplay.GameplayLifecycle
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