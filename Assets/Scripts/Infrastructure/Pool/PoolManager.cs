using System;
using System.Collections.Generic;
using ProjectTemplate.Infrastructure.Pool.Enums;
using UnityEngine;
using UnityEngine.Pool;
using VContainer.Unity;

namespace ProjectTemplate.Infrastructure.Pool
{
	public class PoolManager : IInitializable
	{
		private readonly Dictionary<PoolID, object> _pools = new();
		private readonly PoolConfig _config;

		public PoolManager(PoolConfig config)
		{
			_config = config;
		}

		public void Initialize()
		{
			foreach (PoolConfig.PoolEntry entry in _config.PoolEntries)
			{
				if (entry.IsMonoBehaviour && entry.Prefab != null)
				{
					// Use ObjectPool for MonoBehaviour-based prefabs
					ObjectPool<GameObject> pool = new(
					                                  () => GameObject.Instantiate(entry.Prefab),
					                                  obj => obj.SetActive(true),
					                                  obj => obj.SetActive(false),
					                                  GameObject.Destroy,
					                                  false,
					                                  10);

					_pools[Enum.Parse<PoolID>(entry.PoolID)] = pool;
				}
				else if (!entry.IsMonoBehaviour && entry.ClassType != null)
				{
					// Use ObjectPool for plain C# classes
					Type poolType = typeof(ObjectPool<>).MakeGenericType(entry.ClassType);
					Func<object> createFunc = Delegate.CreateDelegate(
					                                                  typeof(Func<>).MakeGenericType(entry.ClassType),
					                                                  this,
					                                                  nameof(CreateInstance)) as Func<object>;

					object pool = Activator.CreateInstance(poolType, createFunc, null, null, false, 10);
					_pools[Enum.Parse<PoolID>(entry.PoolID)] = pool;
				}
			}
		}

		private object CreateInstance(Type type) => Activator.CreateInstance(type);

		public T Get<T>(PoolID id) where T : class
		{
			if (_pools.TryGetValue(id, out object pool) && pool is ObjectPool<T> objectPool) return objectPool.Get();
			throw new Exception($"No pool found for ID {id}");
		}

		public void Release<T>(PoolID id, T obj) where T : class
		{
			if (_pools.TryGetValue(id, out object pool) && pool is ObjectPool<T> objectPool)
				objectPool.Release(obj);
			else
				Debug.LogError($"No pool found for ID {id}");
		}
	}
}