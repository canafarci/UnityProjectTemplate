using Lofelt.NiceVibrations;
using ProjectTemplate.Runtime.CrossScene.Signals;
using ProjectTemplate.Runtime.Gameplay.Enums;
using ProjectTemplate.Runtime.Gameplay.Signals;
using ProjectTemplate.Runtime.Infrastructure.Pool;
using ProjectTemplate.Runtime.Infrastructure.Signals;
using VContainer;
using VContainer.Unity;

namespace ProjectTemplate.Runtime.Gameplay.GameplayLifecycle
{
	public class GameplayEntryPoint : IStartable
	{
		[Inject] private SignalBus _signalBus;
		[Inject] private PoolManager _poolManager;
		
		public void Start()
		{
			_signalBus.Fire(new ChangeGameStateSignal(GameState.Initializing));
			
			InitializeGameplay();
			
			_signalBus.Fire(new ChangeGameStateSignal(GameState.Playing));
		}

		private void InitializeGameplay()
		{
			_signalBus.Fire(new PlayHapticSignal(HapticPatterns.PresetType.HeavyImpact));
		}
	}
}