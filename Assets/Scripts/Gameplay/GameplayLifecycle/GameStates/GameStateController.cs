using ProjectTemplate.Gameplay.Enums;
using ProjectTemplate.Gameplay.Signals;
using ProjectTemplate.Infrastructure.Templates;

namespace ProjectTemplate.Gameplay.GameplayLifecycle.GameStates
{
	public class GameStateController : SignalListener
	{
		private GameState _currentState;

		protected override void SubscribeToSignals()
		{
			_signalBus.Subscribe<ChangeGameStateSignal>(OnChangeGameStateSignal);
		}

		private void OnChangeGameStateSignal(ChangeGameStateSignal signal)
		{
			GameState oldState = _currentState;
			_currentState = signal.newState;
			
			_signalBus.Fire(new GameStateChangedSignal(_currentState, oldState));
		}
		
		protected override void UnsubscribeToSignals()
		{
			_signalBus.Unsubscribe<ChangeGameStateSignal>(OnChangeGameStateSignal);
		}
	}
}