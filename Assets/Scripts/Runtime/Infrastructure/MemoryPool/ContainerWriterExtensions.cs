using System;
using System.Collections.Generic;

using VContainer;
using VContainer.Unity;

using ProjectTemplate.Runtime.Infrastructure.MemoryPool;

namespace ProjectTemplate.Runtime.Infrastructure.MemoryPool
{
	public static class ContainerBuilderExtensions
	{
		public static void RegisterPoolManager(this IContainerBuilder builder, PoolConfig poolConfig)
		{
			builder.RegisterEntryPoint<PoolManager>(resolver => new PoolManager(poolConfig), Lifetime.Singleton).AsSelf();
		}
	}
}