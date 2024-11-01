using VContainer;
using VContainer.Unity;

using ProjectTemplate.Gameplay.GameplayLifecycle;
using ProjectTemplate.Gameplay.GameplayLifecycle.GameStates;
using ProjectTemplate.Gameplay.UI.GameOverPanel;
using ProjectTemplate.Gameplay.UI.GameSceneSettings;

namespace ProjectTemplate.Scopes
{
	public class GameplayScope : LifetimeScope
	{
		protected override void Configure(IContainerBuilder builder)
		{
			RegisterGameplayLifecycleManagers(builder);
			RegisterInGameSettings(builder);
			RegisterGameOverPanel(builder);
		}

		private void RegisterGameplayLifecycleManagers(IContainerBuilder builder)
		{
			builder.RegisterEntryPoint<GameplayEntryPoint>();
			builder.Register<GameplayExitPoint>(Lifetime.Singleton);
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
	}
}