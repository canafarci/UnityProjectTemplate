using UnityEngine;
using UnityEngine.Pool;

namespace ProjectTemplate.Runtime.Infrastructure.Pool
{
	public class GenericMonoPool<T> where T : MonoBehaviour
	{
		private readonly GameObject _prefab;
		private readonly ObjectPool<T> _monoPool;

		public GenericMonoPool(GameObject prefab)
		{
			_prefab = prefab;
			
			_monoPool = new ObjectPool<T>(OnCreate,
			                              OnTakeFromPool,
			                              OnReturnToPool,
			                              OnDestroyMono,
			                              true, 10, 100);
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
			return mono;
		}

		private void OnTakeFromPool(T mono)
		{
			mono.gameObject.SetActive(true);
		}

		private void OnReturnToPool(T mono)
		{
			mono.gameObject.SetActive(false);
		}

		private void OnDestroyMono(T mono)
		{
			GameObject.Destroy(mono.gameObject);
		}
	}
}