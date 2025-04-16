using Cysharp.Threading.Tasks;
using ProjectTemplate.Runtime.CrossScene.Signals;
using ProjectTemplate.Runtime.Infrastructure.ApplicationState;
using ProjectTemplate.Runtime.Infrastructure.ApplicationState.Signals;
using ProjectTemplate.Runtime.Infrastructure.Initialization;
using ProjectTemplate.Runtime.Infrastructure.Initialization.Enums;
using ProjectTemplate.Runtime.Infrastructure.Templates;

namespace ProjectTemplate.Runtime.MainMenu.MainMenuLifecycle
{
	public class MainMenuEntryPoint : SceneEntryPoint
	{
		private readonly SceneInitializer _sceneInitializer;

		public MainMenuEntryPoint(SceneInitializer sceneInitializer)
		{
			_sceneInitializer = sceneInitializer;
		}
		
		protected override async UniTaskVoid EnterScene()
		{
			_signalBus.Fire(new ChangeAppStateSignal(AppStateID.MainMenu));
			await _sceneInitializer.InitializeModules(ModuleDomain.MainMenu);
		}
	}
}