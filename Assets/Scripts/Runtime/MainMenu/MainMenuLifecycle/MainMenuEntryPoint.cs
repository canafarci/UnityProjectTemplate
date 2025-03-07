using ProjectTemplate.Runtime.CrossScene.Signals;
using ProjectTemplate.Runtime.Gameplay.GameplayLifecycle.Signals;
using ProjectTemplate.Runtime.Infrastructure.ApplicationState;
using ProjectTemplate.Runtime.Infrastructure.ApplicationState.Signals;
using ProjectTemplate.Runtime.Infrastructure.Templates;

namespace ProjectTemplate.Runtime.MainMenu.MainMenuLifecycle
{
	public class MainMenuEntryPoint : SceneEntryPoint
	{
		private readonly MainMenuInitializer _mainMenuInitializer;

		public MainMenuEntryPoint(MainMenuInitializer mainMenuInitializer)
		{
			_mainMenuInitializer = mainMenuInitializer;
		}
		
		protected override async void EnterScene()
		{
			_signalBus.Fire(new ChangeAppStateSignal(AppStateID.MainMenu));
			await _mainMenuInitializer.InitializeModules();
		}
	}
}