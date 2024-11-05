using ProjectTemplate.Runtime.Gameplay.Enums;
using ProjectTemplate.Runtime.Gameplay.Signals;
using ProjectTemplate.Runtime.Infrastructure.Templates;

namespace ProjectTemplate.Runtime.Gameplay.GameplayLifecycle.GameStates
{
	public class GameStateController : SignalListener
	{
		private readonly IGameStateModel _gameStateModel;
		private GameState _currentState;

		public GameStateController(IGameStateModel gameStateModel)
		{
			_gameStateModel = gameStateModel;
		}

		protected override void SubscribeToEvents()
		{
			_signalBus.Subscribe<ChangeGameStateSignal>(OnChangeGameStateSignal);
			_signalBus.Subscribe<SetGameResultSignal>(OnSetGameResultSignal);
		}
		
		private void OnSetGameResultSignal(SetGameResultSignal signal)
		{
			_gameStateModel.SetGameIsWon(signal.isGameWon);
		}

		private void OnChangeGameStateSignal(ChangeGameStateSignal signal)
		{
			GameState oldState = _currentState;
			_currentState = signal.newState;
			
			_signalBus.Fire(new GameStateChangedSignal(_currentState, oldState));
		}
		
		protected override void UnsubscribeFromEvents()
		{
			_signalBus.Unsubscribe<ChangeGameStateSignal>(OnChangeGameStateSignal);
			_signalBus.Unsubscribe<SetGameResultSignal>(OnSetGameResultSignal);
		}
	}
}