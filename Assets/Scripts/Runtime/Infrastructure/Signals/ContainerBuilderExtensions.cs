using VContainer;

namespace ProjectTemplate.Runtime.Infrastructure.Signals
{
	public static class ContainerBuilderExtensions
	{
		public static void DeclareSignal<TSignal>(this IContainerBuilder builder)
		{
			// Register a build callback to declare the signal when the container is built
			builder.RegisterBuildCallback(container =>
			{
				var signalBus = container.Resolve<SignalBus>();
				signalBus.DeclareSignal<TSignal>();
			});
		}

		public static void RegisterSignalBus(this IContainerBuilder builder)
		{
			// Register the SignalBus as a singleton
			builder.Register<SignalBus>(Lifetime.Singleton);
		}

	}
}