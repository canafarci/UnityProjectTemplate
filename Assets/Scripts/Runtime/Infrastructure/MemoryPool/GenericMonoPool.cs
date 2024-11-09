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
		private List<T> _initialObjectList = new List<T>();
		private readonly AppStateID _appStateToRecycle;
		private readonly bool _dontRecycleWithSceneChange;

		public GenericMonoPool(GameObject prefab, PoolParams poolParams)
		{
			_prefab = prefab;
			_initialSize = poolParams.InitialSize;
			_appStateToRecycle = poolParams.RecycleSceneID;
			_dontRecycleWithSceneChange = poolParams.DontRecycleWithSceneChange;
			
			_monoPool = new ObjectPool<T>(OnCreate,
			                              OnGetFromPool,
			                              OnReturnToPool,
			                              OnDestroyMono,
			                              true, 
			                              poolParams.DefaultCapacity, 
			                              poolParams.MaximumSize);
			
			if (_dontRecycleWithSceneChange)
				InstantiateDefaultObjects();
		}

		public AppStateID appStateToRecycle => _appStateToRecycle;
		public bool dontRecycleWithSceneChange => _dontRecycleWithSceneChange;

		//instantiate att app launch & dont destroy
		//instantiate based on scene type and recycle on scene change
		
		public void InstantiateDefaultObjects()
		{
			for (int i = 0; i < _initialSize; i++)
			{
				T mono = OnCreate();
				_initialObjectList.Add(mono);
			}
			
			_initialObjectList.ForEach(Release);
			
			_initialObjectList.Clear();
			_initialObjectList = null;
		}
		
		public void DestroyAll()
		{
			foreach (T mono in GetInternalList())
			{
				OnDestroyMono(mono);
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

		private T OnCreate()
		{
			T mono = GameObject.Instantiate(_prefab).GetComponent<T>();
			MonoBehaviour.DontDestroyOnLoad(mono.gameObject);
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
	}
}