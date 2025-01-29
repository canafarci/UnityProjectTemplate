using System;
using System.Collections.Generic;
using ProjectTemplate.Runtime.Infrastructure.ApplicationState;
using ProjectTemplate.Runtime.Infrastructure.ApplicationState.Signals;
using ProjectTemplate.Runtime.Infrastructure.MemoryPool.Data;
using ProjectTemplate.Runtime.Infrastructure.Templates;
using UnityEngine;
using UnityEngine.Assertions;

namespace ProjectTemplate.Runtime.Infrastructure.MemoryPool
{
public class PoolManager : SignalListener
{
	private readonly Dictionary<Type, object> _monoPools = new();
	private readonly Dictionary<Type, object> _genericPools = new();
	private readonly PoolConfig _config;
	private Transform _poolParent;

	//needed a static event to subscribe to this using a generic object's constructor.
	//could be encapsulated further by making generic pool classes private 
	internal static event Action<AppStateID, AppStateID> OnAppStateChanged;

	public PoolManager(PoolConfig config)
	{
		_config = config;
	}
	
	protected override void SubscribeToEvents()
	{
		_signalBus.Subscribe<AppStateChangedSignal>(OnAppStateChangedSignal);
	}

	private void OnAppStateChangedSignal(AppStateChangedSignal signal)
	{
		OnAppStateChanged?.Invoke(signal.newState, signal.oldState);
	}

	protected override void UnsubscribeFromEvents()
	{
		_signalBus.Unsubscribe<AppStateChangedSignal>(OnAppStateChangedSignal);
	}

	public override void Initialize()
	{
		base.Initialize();

		CreateDontDestroyOnLoadParent();

		InitializePoolLookup();
	}

	private void CreateDontDestroyOnLoadParent()
	{
		_poolParent = new GameObject("Pool Parent").transform;
		GameObject.DontDestroyOnLoad(_poolParent);
	}

	private void InitializePoolLookup()
	{
		foreach (PoolEntry entry in _config.PoolEntries)
		{
			if (entry.IsMonoBehaviour && entry.Prefab != null && entry.classType != null)
			{
				CreatePoolEntry(entry, typeof(GenericMonoPool<>), _monoPools, entry.Prefab, _poolParent);
			}
			else if (!entry.IsMonoBehaviour && entry.classType != null)
			{
				CreatePoolEntry(entry, typeof(GenericPurePool<>), _genericPools);
			}
		}
	}

	private void CreatePoolEntry(
		PoolEntry entry,
		Type genericBaseType,
		Dictionary<Type, object> poolDictionary,
		params object[] additionalConstructorArgs)
	{
		Type poolType = genericBaseType.MakeGenericType(entry.classType);

		PoolParams poolParams = new PoolParams(entry.InitialSize,
		                                       entry.DefaultCapacity,
		                                       entry.MaximumSize,
		                                       entry.ManagePoolOnSceneChange,
		                                       entry.LifetimeSceneID);

		// combine the additional args with the poolParams
		List<object> constructorArgs = new List<object>(additionalConstructorArgs) { poolParams };

		object pool = Activator.CreateInstance(poolType, constructorArgs.ToArray());
		poolDictionary[entry.classType] = pool;
	}

	public T GetMono<T>() where T : MonoBehaviour, IPoolable
	{
		if (_monoPools.TryGetValue(typeof(T), out object poolObj))
		{
			var pool = poolObj as GenericMonoPool<T>;
			Assert.IsNotNull(pool, "Pool object is null.");

			return pool.Get();
		}
		
		throw new InvalidOperationException($"No pool found for type {typeof(T)}.");
	}
	
	public T GetPure<T>() where T : class, IPoolable
	{
		if (_genericPools.TryGetValue(typeof(T), out object poolObj))
		{
			GenericPurePool<T> purePool = poolObj as GenericPurePool<T>;
			Assert.IsNotNull(purePool, "Pool object is null.");
			return purePool.Get();
		}
		
		throw new InvalidOperationException($"No pool found for type {typeof(T)}.");
	}

	public void ReleaseMono<T>(T obj) where T : MonoBehaviour, IPoolable
	{
		if (_monoPools.TryGetValue(typeof(T), out object poolObj))
		{
			GenericMonoPool<T> pool = poolObj as GenericMonoPool<T>;
			Assert.IsNotNull(pool, "Pool object is null.");

			pool.Release(obj);
		}
		else
			Debug.LogError($"No pool found for type {typeof(T)}.");
	}
	
	public void ReleasePure<T>(T obj) where T : class, IPoolable
	{
		if (_genericPools.TryGetValue(typeof(T), out object poolObj))
		{
			GenericPurePool<T> purePool = poolObj as GenericPurePool<T>;
			Assert.IsNotNull(purePool, "Pool object is null.");

			purePool.Release(obj);
		}
		else
			Debug.LogError($"No pool found for type {typeof(T)}.");
	}

	public void ReleaseAllMono<T>() where T : MonoBehaviour, IPoolable
	{
		if (_monoPools.TryGetValue(typeof(T), out object poolObj))
		{
			GenericMonoPool<T> pool = poolObj as GenericMonoPool<T>;
			Assert.IsNotNull(pool, "Pool object is null.");

			pool.ReleaseAll();
		}
		else
			Debug.LogError($"No pool found for type {typeof(T)}.");
	}
	
	public void ReleaseAllPure<T>() where T : class, IPoolable
	{
		if (_genericPools.TryGetValue(typeof(T), out object poolObj))
		{
			GenericPurePool<T> purePool = poolObj as GenericPurePool<T>;
			Assert.IsNotNull(purePool, "Pool object is null.");

			purePool.ReleaseAll();
		}
		else
			Debug.LogError($"No pool found for type {typeof(T)}.");
	}
}
}