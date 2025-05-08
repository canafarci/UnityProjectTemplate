using ProjectTemplate.Runtime.CrossScene.Progress;
using ProjectTemplate.Runtime.Gameplay.GameplayLifecycle.Enums;
using ProjectTemplate.Runtime.Gameplay.GameplayLifecycle.GameStates;
using ProjectTemplate.Runtime.Gameplay.Signals;
using ProjectTemplate.Runtime.Infrastructure.Templates;

namespace ProjectTemplate.Runtime.Gameplay.UI.ProgressionPanel
{
	public class ProgressionController : SignalListener
	{
		private readonly ProgressionService _service;
		private readonly ProgressionMediator _uiMediator;
		private readonly IGameStateModel _gameStateModel;

		public ProgressionController(
			ProgressionService service,
			ProgressionMediator uiMediator,
			IGameStateModel gameStateModel)
		{
			_service = service;
			_uiMediator = uiMediator;
			_gameStateModel = gameStateModel;
		}

		protected override void SubscribeToEvents()
		{
			_signalBus.Subscribe<GameStateChangedSignal>(OnStateChanged);

			// Must match Action<int,int> signature
			_service.OnProgressIncreased += HandleProgressIncreased;
			// Action<int>
			_service.OnUnlockedAll += HandleUnlockedAll;
		}

		private void OnStateChanged(GameStateChangedSignal signal)
		{
			if (signal.newState == GameState.GameOver && _gameStateModel.isGameWon)
				_service.HandleLevelWin();
		}

		// Matches Action<int,int>
		private void HandleProgressIncreased(int newIndex, int targetCount)
		{
			_uiMediator.ShowProgress(unlocked: false);
		}

		// Matches Action<int>
		private void HandleUnlockedAll(int targetCount)
		{
			_uiMediator.ShowProgress(unlocked: true);
		}

		protected override void UnsubscribeFromEvents()
		{
			_signalBus.Unsubscribe<GameStateChangedSignal>(OnStateChanged);

			_service.OnProgressIncreased -= HandleProgressIncreased;
			_service.OnUnlockedAll -= HandleUnlockedAll;
		}
	}
}