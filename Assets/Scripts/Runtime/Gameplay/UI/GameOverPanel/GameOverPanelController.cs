using Lofelt.NiceVibrations;
using ProjectTemplate.Runtime.CrossScene.Data;
using ProjectTemplate.Runtime.CrossScene.Signals;
using ProjectTemplate.Runtime.Gameplay.GameplayLifecycle.Enums;
using ProjectTemplate.Runtime.Gameplay.GameplayLifecycle.GameStates;
using ProjectTemplate.Runtime.Gameplay.Signals;
using ProjectTemplate.Runtime.Infrastructure.Templates;

namespace ProjectTemplate.Runtime.Gameplay.UI.GameOverPanel
{
    public class GameOverPanelController : SignalListener
    {
        private readonly GameOverPanelMediator   _mediator;
        private readonly IGameStateModel         _gameStateModel;
        private readonly IGameplayPersistentData _gameplayPersistentData;

        public GameOverPanelController(
            GameOverPanelMediator mediator,
            IGameStateModel gameStateModel,
            IGameplayPersistentData gameplayPersistentData)
        {
            _mediator               = mediator;
            _gameStateModel         = gameStateModel;
            _gameplayPersistentData = gameplayPersistentData;
        }

        protected override void SubscribeToEvents()
        {
            _signalBus.Subscribe<GameStateChangedSignal>(OnGameStateChanged);
            _mediator.OnContinueButtonClicked += HandleContinueClicked;
        }

        private void OnGameStateChanged(GameStateChangedSignal signal)
        {
            if (signal.newState == GameState.GameOver)
            {
                _mediator.ActivateGameOverPanel(
                    _gameStateModel.isGameWon,
                    _gameplayPersistentData.levelVisualDisplayNumber
                );

                HapticPatterns.PresetType preset = _gameStateModel.isGameWon
                    ? HapticPatterns.PresetType.Success
                    : HapticPatterns.PresetType.Failure;
                _signalBus.Fire(new PlayHapticSignal(preset));
            }
        }

        private void HandleContinueClicked()
        {
            _signalBus.Fire(new PlayHapticSignal(HapticPatterns.PresetType.MediumImpact));
            _signalBus.Fire(new TriggerExitGameplayLevelSignal());
            _mediator.DisableContinueButton();
        }

        protected override void UnsubscribeFromEvents()
        {
            _signalBus.Unsubscribe<GameStateChangedSignal>(OnGameStateChanged);
            _mediator.OnContinueButtonClicked -= HandleContinueClicked;
        }
    }
}
