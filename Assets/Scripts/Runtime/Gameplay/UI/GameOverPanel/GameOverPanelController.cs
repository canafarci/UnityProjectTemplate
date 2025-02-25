using Lofelt.NiceVibrations;
using ProjectTemplate.Runtime.CrossScene.Data;
using ProjectTemplate.Runtime.CrossScene.Enums;
using ProjectTemplate.Runtime.CrossScene.Progress;
using ProjectTemplate.Runtime.CrossScene.Signals;
using ProjectTemplate.Runtime.Gameplay.Enums;
using ProjectTemplate.Runtime.Gameplay.GameplayLifecycle.Enums;
using ProjectTemplate.Runtime.Gameplay.GameplayLifecycle.GameStates;
using ProjectTemplate.Runtime.Gameplay.Signals;
using ProjectTemplate.Runtime.Infrastructure.Templates;

namespace ProjectTemplate.Runtime.Gameplay.UI.GameOverPanel
{
	public class GameOverPanelController : SignalListener
	{
		private readonly GameOverPanelMediator _mediator;
		private readonly IGameStateModel _gameStateModel;
		private readonly IGameplayPersistentData _gameplayPersistentData;
		private readonly IProgressModel _progressModel;

		public GameOverPanelController(GameOverPanelMediator mediator,
			IGameStateModel gameStateModel,
			IGameplayPersistentData gameplayPersistentData,
			IProgressModel progressModel)
		{
			_mediator = mediator;
			_gameStateModel = gameStateModel;
			_gameplayPersistentData = gameplayPersistentData;
			_progressModel = progressModel;
		}
		
		protected override void SubscribeToEvents()
		{
			 _signalBus.Subscribe<GameStateChangedSignal>(OnGameStateChangedSignal);
			 _mediator.OnContinueButtonClicked += ContinueButtonClickedHandler;
		}

		private void ContinueButtonClickedHandler()
		{
			_signalBus.Fire(new PlayHapticSignal(HapticPatterns.PresetType.MediumImpact));
			
			_signalBus.Fire(new TriggerExitGameplayLevelSignal());
			_mediator.DisableContinueButton();
		}

		private void OnGameStateChangedSignal(GameStateChangedSignal signal)
		{
			if (signal.newState == GameState.GameOver)
			{
				_mediator.ActivateGameOverPanel(_gameStateModel.isGameWon, _gameplayPersistentData.levelVisualDisplayNumber);

				UpdateProgress();

				HapticPatterns.PresetType hapticPreset = _gameStateModel.isGameWon ? HapticPatterns.PresetType.Success : HapticPatterns.PresetType.Failure;
				_signalBus.Fire(new PlayHapticSignal(hapticPreset));
			}
		}

		private void UpdateProgress()
		{
			bool isProgressStepUnlocked = false;
			
			if (_gameStateModel.isGameWon)
			{
				_progressModel.IncrementProgress(out bool unlocked);
				isProgressStepUnlocked = unlocked;
			}
				
			_mediator.UpdateProgressBar(_gameStateModel.isGameWon,  isProgressStepUnlocked);
		}

		protected override void UnsubscribeFromEvents()
		{
			_signalBus.Unsubscribe<GameStateChangedSignal>(OnGameStateChangedSignal);
			_mediator.OnContinueButtonClicked -= ContinueButtonClickedHandler;
		}
	}
}