using System;
using UnityEngine.Pool;

namespace ProjectTemplate.Infrastructure.Pool
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
			T mono = _genericPool.Get();
			return mono;
		}

		public void Release(T mono)
		{
			_genericPool.Release(mono);
		}

		private T OnCreate()
		{
			T obj = Activator.CreateInstance<T>();
			obj.OnCreate();
			return obj;
		}

		private void OnTakeFromPool(T obj)
		{
			obj.OnTakeFromPool();
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