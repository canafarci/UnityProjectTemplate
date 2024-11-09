using System;
using System.Collections.Generic;
using System.Reflection;
using ProjectTemplate.Runtime.Infrastructure.ApplicationState;
using UnityEngine;
using UnityEngine.Pool;

namespace ProjectTemplate.Runtime.Infrastructure.MemoryPool
{
	public class GenericMonoPool<T> where T : MonoBehaviour, IPoolable
	{
		private readonly GameObject _prefab;
		private readonly ObjectPool<T> _monoPool;
		private readonly int _initialSize;
		private readonly AppStateID _appStateToRecycle;
		private readonly bool _recycleWithSceneChange;
		
		private List<T> _initialObjectList = new List<T>();

		public GenericMonoPool(GameObject prefab, PoolParams poolParams)
		{
			_prefab = prefab;
			_initialSize = poolParams.InitialSize;
			_appStateToRecycle = poolParams.RecycleSceneID;
			_recycleWithSceneChange = poolParams.RecycleWithSceneChange;
			
			_monoPool = new ObjectPool<T>(OnCreate,
			                              OnGetFromPool,
			                              OnReturnToPool,
			                              OnDestroyMono,
			                              true, 
			                              poolParams.DefaultCapacity, 
			                              poolParams.MaximumSize);

			if (_recycleWithSceneChange)
			{
				PoolManager.OnAppStateChanged += OnAppStateChangedHandler;
			}
			else
			{
				InstantiateDefaultObjects();
			}
		}
		
		public T Get()
		{
			T mono = _monoPool.Get();
			return mono;
		}

		public void Release(T mono)
		{
			_monoPool.Release(mono);
		}

		private void OnAppStateChangedHandler(AppStateID newState, AppStateID oldState)
		{
			if (oldState == _appStateToRecycle)
			{
				DestroyAll();
			}
			else if (newState == _appStateToRecycle)
			{
				InstantiateDefaultObjects();
			}
		}
		
		private void InstantiateDefaultObjects()
		{
			for (int i = 0; i < _initialSize; i++)
			{
				T mono = OnCreate();
				_initialObjectList.Add(mono);
			}
			
			_initialObjectList.ForEach(Release);
			
			_initialObjectList.Clear();

			if (!_recycleWithSceneChange)
				_initialObjectList = null;
		}
		
		private void DestroyAll()
		{
			foreach (T mono in GetInternalList())
			{
				OnDestroyMono(mono);
			}
		}
		
		private T OnCreate()
		{
			T mono = GameObject.Instantiate(_prefab).GetComponent<T>();
			mono.OnCreate();
			return mono;
		}

		private void OnGetFromPool(T mono)
		{
			mono.OnGetFromPool();
			mono.gameObject.SetActive(true);
		}

		private void OnReturnToPool(T mono)
		{
			mono.OnReturnToPool();
			mono.gameObject.SetActive(false);
		}

		private void OnDestroyMono(T mono)
		{
			if (mono == null) return;
			
			mono.OnDestroy();
			GameObject.Destroy(mono.gameObject);
		}

		private List<T> GetInternalList()
		{
			// Use reflection to access the private `m_List` field in `_monoPool`
			FieldInfo listField = typeof(ObjectPool<T>).GetField("m_List", BindingFlags.NonPublic | BindingFlags.Instance);

			if (listField != null)
			{
				return (List<T>)listField.GetValue(_monoPool);
			}
			else
			{
				throw new InvalidOperationException("Field 'm_List' not found.");
			}
		}

		~GenericMonoPool()
		{
			DestroyAll();

			if (_recycleWithSceneChange)
			{
				PoolManager.OnAppStateChanged -= OnAppStateChangedHandler;
			}
		}
	}
}