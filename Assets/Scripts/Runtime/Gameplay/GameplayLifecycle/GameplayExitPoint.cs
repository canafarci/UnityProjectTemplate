using ProjectTemplate.Runtime.CrossScene.Currency;
using ProjectTemplate.Runtime.CrossScene.Data;
using ProjectTemplate.Runtime.CrossScene.Enums;
using ProjectTemplate.Runtime.CrossScene.Scenes.Enums;
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
		private readonly ICurrencyModel _currencyModel;

		public GameplayExitPoint(ApplicationSettings applicationSettings,
		                         IGameStateModel gameStateModel,
		                         IGameplayPersistentData gameplayPersistentData,
		                         ICurrencyModel currencyModel)
		{
			_applicationSettings = applicationSettings;
			_gameStateModel = gameStateModel;
			_gameplayPersistentData = gameplayPersistentData;
			_currencyModel = currencyModel;
		}		
		
		protected override void SubscribeToEvents()
		{
			_signalBus.Subscribe<TriggerExitGameplayLevelSignal>(OnExitGameplayLevelSignal);
		}

		private void OnExitGameplayLevelSignal(TriggerExitGameplayLevelSignal signal)
		{
			if (signal.reload)
			{
				//reload can only be called from gameplay levels
				_signalBus.Fire(new LoadSceneSignal(SceneID.Gameplay));
			}
			else
			{
				SceneID targetSceneID = GetNextLevelIndex();
				_signalBus.Fire(new LoadSceneSignal(targetSceneID));
			}
		}
		
		private SceneID GetNextLevelIndex()
		{
			if (_gameStateModel.isGameWon)
			{
				_gameplayPersistentData.IncreaseTargetSceneIndex();
				_currencyModel.AddCurrency(CurrencyID.Money, 10);

			}

			SceneID sceneID = _applicationSettings.HasMainMenu ?
				                                            SceneID.MainMenu :
				                                            SceneID.Gameplay;
			return sceneID;
		}

		protected override void UnsubscribeFromEvents()
		{
			_signalBus.Unsubscribe<TriggerExitGameplayLevelSignal>(OnExitGameplayLevelSignal);
		}
	}
}