using ProjectTemplate.Runtime.Bootstrap;
using ProjectTemplate.Runtime.Infrastructure.Data;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace ProjectTemplate.Runtime.Scopes
{
	public class BootstrapScope : LifetimeScope
	{
		[SerializeField] private ApplicationSettings ApplicationSettings;
		protected override void Configure(IContainerBuilder builder)
		{
			builder.RegisterEntryPoint<BootstrapSceneEntryPoint>().AsSelf();
			
			builder.Register<AppInitializer>(Lifetime.Singleton).AsSelf();
		}
	}
}