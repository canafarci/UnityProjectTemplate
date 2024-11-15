using ProjectTemplate.Runtime.Gameplay.Enums;
using ProjectTemplate.Runtime.Gameplay.GameplayLifecycle.GameStates;
using ProjectTemplate.Runtime.Gameplay.Signals;
using ProjectTemplate.Runtime.Infrastructure.Templates;

namespace ProjectTemplate.Runtime.Gameplay.UI.GameOverPanel
{
	public class GameOverPanelController : SignalListener
	{
		private readonly GameOverPanelMediator _mediator;
		private readonly IGameStateModel _gameStateModel;

		public GameOverPanelController(GameOverPanelMediator mediator, IGameStateModel gameStateModel)
		{
			_mediator = mediator;
			_gameStateModel = gameStateModel;
		}
		
		protected override void SubscribeToEvents()
		{
			 _signalBus.Subscribe<GameStateChangedSignal>(OnGameStateChangedSignal);
			 _mediator.OnContinueButtonClicked += ContinueButtonClickedHandler;
		}

		private void ContinueButtonClickedHandler()
		{
			_signalBus.Fire(new TriggerExitGameplayLevelSignal());
			_mediator.DisableContinueButton();
		}

		private void OnGameStateChangedSignal(GameStateChangedSignal signal)
		{
			if (signal.newState == GameState.GameOver)
			{
				_mediator.ActivateGameOverPanel(_gameStateModel.isGameWon);
			}
		}

		protected override void UnsubscribeFromEvents()
		{
			_signalBus.Unsubscribe<GameStateChangedSignal>(OnGameStateChangedSignal);
			_mediator.OnContinueButtonClicked -= ContinueButtonClickedHandler;
		}
	}
}