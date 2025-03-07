using ProjectTemplate.Runtime.Gameplay.GameplayLifecycle.Signals;
using ProjectTemplate.Runtime.Infrastructure.Signals;
using ProjectTemplate.Runtime.MainMenu;
using ProjectTemplate.Runtime.MainMenu.Enums;
using ProjectTemplate.Runtime.MainMenu.MainMenuLifecycle;
using ProjectTemplate.Runtime.MainMenu.Signals;
using ProjectTemplate.Runtime.MainMenu.UI.CurrencyPanel;
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
			RegisterCurrencyPanel(builder);
			RegisterSignals(builder);
		}

		private void RegisterCurrencyPanel(IContainerBuilder builder)
		{
			builder.RegisterEntryPoint<CurrencyPanelController>();
			builder.RegisterComponentInHierarchy<CurrencyPanelView>().AsSelf();
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
			builder.RegisterEntryPoint<MainMenuInitializer>().AsSelf();
		}
		
		private void RegisterSettingsPanel(IContainerBuilder builder)
		{
			builder.RegisterEntryPoint<SettingsPanelController>();
			builder.RegisterEntryPoint<SettingsPanelMediator>().AsSelf();
			builder.RegisterComponentInHierarchy<SettingsPanelView>().AsSelf();
		}
		
		private void RegisterSignals(IContainerBuilder builder)
		{
			//Main Menu
			builder.DeclareSignal<TriggerExitMainMenuSignal>();
			builder.DeclareSignal<ModuleInitializedSignal<MainMenuInitializableModule>>();
		}
	}
}