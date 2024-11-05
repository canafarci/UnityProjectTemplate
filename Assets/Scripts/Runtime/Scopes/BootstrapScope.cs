using ProjectTemplate.Runtime.Bootstrap;
using VContainer;
using VContainer.Unity;

namespace ProjectTemplate.Runtime.Scopes
{
	public class BootstrapScope : LifetimeScope
	{
		protected override void Configure(IContainerBuilder builder)
		{
			builder.RegisterEntryPoint<BootstrapSceneEntryPoint>().AsSelf();
		}
	}
}