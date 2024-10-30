using ProjectTemplate.Gameplay.GameSceneSettings;
using VContainer;
using VContainer.Unity;

namespace ProjectTemplate.Scopes
{
	public class GameplayScope : LifetimeScope
	{
		protected override void Configure(IContainerBuilder builder)
		{
			RegisterInGameSettings(builder);
		}

		private void RegisterInGameSettings(IContainerBuilder builder)
		{
			builder.RegisterComponentInHierarchy<GameSceneSettingsView>().AsSelf();
			builder.RegisterEntryPoint<GameSceneSettingsMediator>().AsSelf();
			builder.RegisterEntryPoint<GameSceneSettingsController>().AsSelf();
		}
	}
}