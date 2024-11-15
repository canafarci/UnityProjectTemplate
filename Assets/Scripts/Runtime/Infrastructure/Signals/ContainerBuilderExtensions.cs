using System;
using System.Collections.Generic;
using VContainer;

namespace ProjectTemplate.Runtime.Infrastructure.Signals
{
	public static class ContainerBuilderExtensions
	{
		private static SignalBus _signalBus = null;
		private static List<Type> _applicationScopeSignalTypes = new List<Type>();

		public static void DeclareSignal<TSignal>(this IContainerBuilder builder)
		{
			if (_signalBus == null)
			{
				_applicationScopeSignalTypes.Add(typeof(TSignal));
			}
			else
			{
				_signalBus.DeclareSignal<TSignal>();
			}


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
			builder.Register<SignalBus>(Lifetime.Singleton).WithParameter(_applicationScopeSignalTypes);

			builder.RegisterBuildCallback(container => { _signalBus = container.Resolve<SignalBus>(); });
		}
	}
}