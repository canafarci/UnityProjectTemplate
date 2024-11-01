using ProjectTemplate.Gameplay.Enums;
using ProjectTemplate.Gameplay.Signals;
using ProjectTemplate.Infrastructure.Signals;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace ProjectTemplate.Gameplay.GameplayLifecycle
{
	public class GameplayEntryPoint : IStartable
	{
		[Inject] private SignalBus _signalBus;
		
		public void Start()
		{
			_signalBus.Fire(new ChangeGameStateSignal(GameState.Initializing));
			
			InitializeGameplay();
			
			_signalBus.Fire(new ChangeGameStateSignal(GameState.Playing));
			
			_signalBus.Fire(new SetGameResultSignal(isGameWon: true));
			_signalBus.Fire(new ChangeGameStateSignal(GameState.GameOver));
		}

		private void InitializeGameplay()
		{
		}
	}
}