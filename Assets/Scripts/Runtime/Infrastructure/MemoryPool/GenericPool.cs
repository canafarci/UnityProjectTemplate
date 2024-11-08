using System;
using UnityEngine.Pool;

namespace ProjectTemplate.Runtime.Infrastructure.MemoryPool
{
	public class GenericPool<T> where T : class, IPoolable
	{
		private readonly ObjectPool<T> _genericPool;

		public GenericPool()
		{
			_genericPool = new ObjectPool<T>(OnCreate,
			                                 OnTakeFromPool,
			                                 OnReturnToPool,
			                                 OnDestroy,
			                                 true, 10, 100);
		}

		public T Get()
		{
			T obj = _genericPool.Get();
			return obj;
		}

		public void Release(T obj)
		{
			_genericPool.Release(obj);
		}

		private T OnCreate()
		{
			T obj = Activator.CreateInstance<T>();
			obj.OnCreate();
			return obj;
		}

		private void OnTakeFromPool(T obj)
		{
			obj.OnGetFromPool();
		}

		private void OnReturnToPool(T obj)
		{
			obj.OnReturnToPool();
		}

		private void OnDestroy(T obj)
		{
			obj.OnDestroy();
		}
	}
}