using ProjectTemplate.Runtime.MainMenu.MainMenuLifecycle;
using ProjectTemplate.Runtime.MainMenu.PlayGameCanvas;
using VContainer;
using VContainer.Unity;

namespace ProjectTemplate.Runtime.Scopes
{
	public class MainMenuScope : LifetimeScope
	{
		protected override void Configure(IContainerBuilder builder)
		{
			RegisterMainMenuLifecycleManagers(builder);
			RegisterPlayGameCanvas(builder);
		}

		private void RegisterPlayGameCanvas(IContainerBuilder builder)
		{
			builder.RegisterComponentInHierarchy<PlayGamePanelView>().AsSelf();
			builder.RegisterEntryPoint<PlayGameCanvasController>();
		}

		private void RegisterMainMenuLifecycleManagers(IContainerBuilder builder)
		{
			builder.RegisterEntryPoint<MainMenuEntryPoint>();
			builder.RegisterEntryPoint<MainMenuExitPoint>();
		}
	}
}