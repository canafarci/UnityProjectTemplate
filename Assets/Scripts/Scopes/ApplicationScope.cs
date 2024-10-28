using VContainer;
using VContainer.Unity;

using ProjectTemplate.CrossScene;
using ProjectTemplate.CrossScene.Messages;
using ProjectTemplate.Infrastructure.SignalBus;

namespace ProjectTemplate.Scopes
{
	public class ApplicationScope : LifetimeScope
	{
		protected override void Configure(IContainerBuilder builder)
		{
			RegisterEntryPoints(builder);
			RegisterSignalBus(builder);
			RegisterSignals(builder);
		}
		
		private static void RegisterEntryPoints(IContainerBuilder builder)
		{
			builder.RegisterEntryPoint<SceneLoader>().AsSelf();
		}

		private void RegisterSignalBus(IContainerBuilder builder)
		{
			builder.RegisterSignalBus();
		}
		
		private void RegisterSignals(IContainerBuilder builder)
		{
			builder.DeclareSignal<LoadSceneSignal>();
		}
	}
}