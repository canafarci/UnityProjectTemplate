using ProjectTemplate.Runtime.Gameplay;
using ProjectTemplate.Runtime.Gameplay.GameplayLifecycle;
using ProjectTemplate.Runtime.Gameplay.GameplayLifecycle.GameStates;
using ProjectTemplate.Runtime.Gameplay.UI.GameOverPanel;
using ProjectTemplate.Runtime.Gameplay.UI.GameSceneSettings;
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
	}
}