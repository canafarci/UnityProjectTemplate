using ProjectTemplate.Runtime.Gameplay;
using ProjectTemplate.Runtime.Gameplay.GameplayLifecycle;
using ProjectTemplate.Runtime.Gameplay.GameplayLifecycle.GameStates;
using ProjectTemplate.Runtime.Gameplay.Signals;
using ProjectTemplate.Runtime.Gameplay.UI.GameOverPanel;
using ProjectTemplate.Runtime.Gameplay.UI.GameplayCurrencyPanel;
using ProjectTemplate.Runtime.Gameplay.UI.GameSceneSettings;
using ProjectTemplate.Runtime.Infrastructure.Signals;
using VContainer;
using VContainer.Unity;

namespace ProjectTemplate.Runtime.Scopes
{
	public class GameplayScope : LifetimeScope
	{
		protected override void Configure(IContainerBuilder builder)
		{
			RegisterGameplayLifecycleManagers(builder);
			RegisterInGameSettings(builder);
			RegisterGameOverPanel(builder);
			RegisterCurrencyPanel(builder);
			RegisterSignals(builder);
		}

		private void RegisterGameplayLifecycleManagers(IContainerBuilder builder)
		{
			builder.RegisterEntryPoint<GameplayEntryPoint>();
			builder.RegisterEntryPoint<GameplayExitPoint>();
			builder.RegisterEntryPoint<GameStateController>();
			builder.Register<IGameStateModel, GameStateModel>(Lifetime.Singleton);
		}

		private void RegisterInGameSettings(IContainerBuilder builder)
		{
			builder.RegisterComponentInHierarchy<GameSceneSettingsView>().AsSelf();
			builder.RegisterEntryPoint<GameSceneSettingsMediator>().AsSelf();
			builder.RegisterEntryPoint<GameSceneSettingsController>().AsSelf();
		}
		
		private void RegisterGameOverPanel(IContainerBuilder builder)
		{
			builder.RegisterComponentInHierarchy<GameOverPanelView>().AsSelf();
			builder.RegisterEntryPoint<GameOverPanelMediator>().AsSelf();
			builder.RegisterEntryPoint<GameOverPanelController>().AsSelf();
		}
		
		private void RegisterCurrencyPanel(IContainerBuilder builder)
		{
			builder.RegisterComponentInHierarchy<GameplayCurrencyPanelView>().AsSelf();
			builder.RegisterEntryPoint<GameplayCurrencyPanelController>();
		}
		
		private void RegisterSignals(IContainerBuilder builder)
		{
			builder.DeclareSignal<GameStateChangedSignal>();
			builder.DeclareSignal<ChangeGameStateSignal>();
			builder.DeclareSignal<TriggerLevelEndSignal>();
			builder.DeclareSignal<TriggerExitGameplayLevelSignal>();
		}
	}
}