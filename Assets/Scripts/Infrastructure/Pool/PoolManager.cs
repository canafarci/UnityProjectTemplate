using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Pool;

using System;
using System.Collections.Generic;

using VContainer.Unity;

using ProjectTemplate.Infrastructure.Pool.Enums;

namespace ProjectTemplate.Infrastructure.Pool
{
	public class PoolManager : IInitializable
	{
		private readonly Dictionary<PoolID, object> _monoPools = new();
		private readonly Dictionary<PoolID, object> _genericPools = new();
		private readonly PoolConfig _config;

		public PoolManager(PoolConfig config)
		{
			_config = config;
		}

		public void Initialize()
		{
			foreach (PoolEntry entry in _config.PoolEntries)
			{
				PoolID poolID = Enum.Parse<PoolID>(entry.PoolID);

				if (entry.IsMonoBehaviour && entry.Prefab != null)
				{
					// Get the specific MonoBehaviour type from the prefab
					Type monoType = entry.ClassType;

					// Create GenericMonoPool<T> where T is the MonoBehaviour type
					Type generic = typeof(GenericMonoPool<>);
					Type poolType = generic.MakeGenericType(monoType);

					// Use the constructor that takes a GameObject parameter
					object pool = Activator.CreateInstance(poolType, new object[] { entry.Prefab });

					_monoPools[poolID] = pool;
				}
				else if (!entry.IsMonoBehaviour && entry.ClassType != null)
				{
					// Get the specific MonoBehaviour type from the prefab
					Type type = entry.ClassType;

					// Create GenericMonoPool<T> where T is the MonoBehaviour type
					Type generic = typeof(GenericPool<>);
					Type poolType = generic.MakeGenericType(type);

					// Use the constructor that takes a GameObject parameter
					object pool = Activator.CreateInstance(poolType);
					
					_genericPools[poolID] = pool;
				}
			}
		}

		public T GetMono<T>(PoolID id) where T : MonoBehaviour
		{
			if (_monoPools.TryGetValue(id, out object poolObj))
			{
				var pool = poolObj as GenericMonoPool<T>;
				Assert.IsNotNull(pool, "Pool object is null.");

				return pool.Get();
			}
			
			throw new InvalidOperationException($"No pool found for ID {id} of type {typeof(T)}.");
		}
		
		public T GetGeneric<T>(PoolID id) where T : class, IPoolable
		{
			if (_genericPools.TryGetValue(id, out object poolObj))
			{
				GenericPool<T> pool = poolObj as GenericPool<T>;
				Assert.IsNotNull(pool, "Pool object is null.");
				return pool.Get();
			}
			
			throw new InvalidOperationException($"No pool found for ID {id} of type {typeof(T)}.");
		}

		public void ReleaseMono<T>(PoolID id, T obj) where T : MonoBehaviour
		{
			if (_monoPools.TryGetValue(id, out object poolObj))
			{
				GenericMonoPool<T> pool = poolObj as GenericMonoPool<T>;
				Assert.IsNotNull(pool, "Pool object is null.");

				pool.Release(obj);
			}
			else
				Debug.LogError($"No pool found for ID {id} of type {typeof(T)}.");
		}
		
		public void ReleaseGeneric<T>(PoolID id, T obj) where T : class, IPoolable
		{
			if (_genericPools.TryGetValue(id, out object poolObj))
			{
				GenericPool<T> pool = poolObj as GenericPool<T>;
				Assert.IsNotNull(pool, "Pool object is null.");

				pool.Release(obj);
			}
			else
				Debug.LogError($"No pool found for ID {id} of type {typeof(T)}.");
		}
	}
}