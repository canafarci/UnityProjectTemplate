using VContainer;
using VContainer.Unity;

using ProjectTemplate.Gameplay.Enums;
using ProjectTemplate.Gameplay.Signals;
using ProjectTemplate.Infrastructure.Signals;

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
		}

		private void InitializeGameplay()
		{
		}
	}
}