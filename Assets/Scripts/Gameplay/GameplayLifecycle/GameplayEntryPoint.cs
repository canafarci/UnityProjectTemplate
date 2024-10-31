using ProjectTemplate.Gameplay.Enums;
using ProjectTemplate.Gameplay.Signals;
using ProjectTemplate.Infrastructure.Signals;
using VContainer;
using VContainer.Unity;

namespace ProjectTemplate.Gameplay.GameplayLifecycle
{
	public class GameplayEntryPoint : IInitializable
	{
		[Inject] private SignalBus _signalBus;
		
		public void Initialize()
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