using ProjectTemplate.Runtime.MainMenu.MainMenuLifecycle;
using ProjectTemplate.Runtime.MainMenu.UI.PlayGamePanel;
using ProjectTemplate.Runtime.MainMenu.UI.SettingsPanel;
using VContainer;
using VContainer.Unity;

namespace ProjectTemplate.Runtime.Scopes
{
	public class MainMenuScope : LifetimeScope
	{
		protected override void Configure(IContainerBuilder builder)
		{
			RegisterMainMenuLifecycleManagers(builder);
			RegisterPlayGamePanel(builder);
			RegisterSettingsPanel(builder);
		}

		private void RegisterPlayGamePanel(IContainerBuilder builder)
		{
			builder.RegisterComponentInHierarchy<PlayGamePanelView>().AsSelf();
			builder.RegisterEntryPoint<PlayGamePanelController>();
		}

		private void RegisterMainMenuLifecycleManagers(IContainerBuilder builder)
		{
			builder.RegisterEntryPoint<MainMenuEntryPoint>();
			builder.RegisterEntryPoint<MainMenuExitPoint>();
		}
		
		private void RegisterSettingsPanel(IContainerBuilder builder)
		{
			builder.RegisterEntryPoint<SettingsPanelController>();
			builder.RegisterEntryPoint<SettingsPanelMediator>().AsSelf();
			builder.RegisterComponentInHierarchy<SettingsPanelView>().AsSelf();
		}
	}
}