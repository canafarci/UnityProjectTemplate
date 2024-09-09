using VContainer;
using VContainer.Unity;

using ProjectTemplate.CrossScene;
using ProjectTemplate.CrossScene.Messages;
using ProjectTemplate.Infrastructure.PubSub;

namespace ProjectTemplate.Scopes
{
	public class ApplicationScope : LifetimeScope
	{
		protected override void Configure(IContainerBuilder builder)
		{
			RegisterEntryPoints(builder);
			RegisterMessages(builder);
		}

		private static void RegisterMessages(IContainerBuilder builder)
		{
			builder.RegisterInstance(new MessageChannel<LoadSceneMessage>()).AsImplementedInterfaces();
		}

		private static void RegisterEntryPoints(IContainerBuilder builder)
		{
			builder.RegisterEntryPoint<SceneLoader>().AsSelf();
		}
	}
}