using ProjectTemplate.Runtime.Infrastructure.ApplicationState.Signals;
using ProjectTemplate.Runtime.Infrastructure.Signals;
using VContainer;
using VContainer.Unity;

namespace ProjectTemplate.Runtime.Infrastructure.ApplicationState
{
	public static class ContainerBuilderExtensions
	{
		public static void RegisterAppController(this IContainerBuilder builder)
		{
			builder.RegisterEntryPoint<ApplicationStateController>();
			
			builder.DeclareSignal<ChangeAppStateSignal>();
			builder.DeclareSignal<AppStateChangedSignal>();
		}
	}
}