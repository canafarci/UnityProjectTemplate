using ProjectTemplate.Gameplay.Enums;
using ProjectTemplate.Gameplay.GameplayLifecycle.GameStates;
using ProjectTemplate.Gameplay.Signals;
using ProjectTemplate.Infrastructure.Templates;
using UnityEngine;

namespace ProjectTemplate.Gameplay.UI.GameOverPanel
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
			_signalBus.Fire(new ExitGameplayLevelSignal());
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