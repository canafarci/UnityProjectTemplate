using ProjectTemplate.Runtime.Infrastructure.ApplicationState;
using ProjectTemplate.Runtime.Infrastructure.ApplicationState.Signals;
using ProjectTemplate.Runtime.Infrastructure.Templates;

namespace ProjectTemplate.Runtime.MainMenu.MainMenuLifecycle
{
	public class MainMenuEntryPoint : SceneEntryPoint
	{
		protected override void EnterScene()
		{
			_signalBus.Fire(new ChangeAppStateSignal(AppStateID.MainMenu));

			InitializeMainMenu();
		}

		private void InitializeMainMenu()
		{
			
		}
	}
}