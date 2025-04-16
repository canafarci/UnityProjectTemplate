using Cysharp.Threading.Tasks;
using ProjectTemplate.Runtime.Gameplay.GameplayLifecycle.Enums;
using ProjectTemplate.Runtime.Gameplay.Signals;
using ProjectTemplate.Runtime.Infrastructure.ApplicationState;
using ProjectTemplate.Runtime.Infrastructure.ApplicationState.Signals;
using ProjectTemplate.Runtime.Infrastructure.Initialization;
using ProjectTemplate.Runtime.Infrastructure.Initialization.Enums;
using ProjectTemplate.Runtime.Infrastructure.Templates;

namespace ProjectTemplate.Runtime.Gameplay.GameplayLifecycle
{
	public class GameplayEntryPoint : SceneEntryPoint
	{
		private readonly SceneInitializer _sceneInitializer;

		public GameplayEntryPoint(SceneInitializer sceneInitializer)
		{
			_sceneInitializer = sceneInitializer;
		}

		protected override async UniTaskVoid EnterScene()
		{
			_signalBus.Fire(new ChangeAppStateSignal(AppStateID.Gameplay));
			_signalBus.Fire(new ChangeGameStateSignal(GameState.Initializing));
			
			await _sceneInitializer.InitializeModules(ModuleDomain.Gameplay);
			
			_signalBus.Fire(new ChangeGameStateSignal(GameState.Playing));
		}
	}
}