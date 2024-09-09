using VContainer;
using VContainer.Unity;

using ProjectTemplate.Bootstrap;

namespace ProjectTemplate.Scopes
{
	public class BootstrapScope : LifetimeScope
	{
		protected override void Configure(IContainerBuilder builder)
		{
			builder.RegisterEntryPoint<BootstrapSceneEntryPoint>().AsSelf();
		}
	}
}