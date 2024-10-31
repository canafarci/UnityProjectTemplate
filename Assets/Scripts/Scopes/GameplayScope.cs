using VContainer;
using VContainer.Unity;

using ProjectTemplate.Gameplay.GameplayLifecycle;
using ProjectTemplate.Gameplay.GameplayLifecycle.GameStates;
using ProjectTemplate.Gameplay.GameSceneSettings;

namespace ProjectTemplate.Scopes
{
	public class GameplayScope : LifetimeScope
	{
		protected override void Configure(IContainerBuilder builder)
		{
			RegisterInGameSettings(builder);
			RegisterGameplayLifecycleManagers(builder);
		}
		
		private void RegisterGameplayLifecycleManagers(IContainerBuilder builder)
		{
			builder.RegisterEntryPoint<GameplayEntryPoint>();
			builder.RegisterEntryPoint<GameStateController>();
		}

		private void RegisterInGameSettings(IContainerBuilder builder)
		{
			builder.RegisterComponentInHierarchy<GameSceneSettingsView>().AsSelf();
			builder.RegisterEntryPoint<GameSceneSettingsMediator>().AsSelf();
			builder.RegisterEntryPoint<GameSceneSettingsController>().AsSelf();
		}
	}
}