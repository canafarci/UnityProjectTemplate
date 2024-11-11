using UnityEngine;

namespace ProjectTemplate.Runtime.Infrastructure.MemoryPool
{
	public class GenericMonoPool<T> : PoolBase<T> where T : MonoBehaviour, IPoolable
	{
		private readonly GameObject _prefab;

		public GenericMonoPool(GameObject prefab, PoolParams poolParams) : base(poolParams)
		{
			_prefab = prefab;

			if (!_managePoolOnSceneChange)
			{
				InstantiateDefaultObjects();
			}
		}

		protected override T CreateObject()
		{
			T mono = GameObject.Instantiate(_prefab).GetComponent<T>();
			mono.OnCreated();
			
			if (!_managePoolOnSceneChange)
				GameObject.DontDestroyOnLoad(mono);
			
			return mono;
		}

		protected override void GetFromPool(T mono)
		{
			mono.OnGetFromPool();
			mono.gameObject.SetActive(true);
		}

		protected override void ReturnToPool(T mono)
		{
			mono.OnReturnToPool();
			mono.gameObject.SetActive(false);
		}

		protected override void DestroyObject(T mono)
		{
			mono.OnDestroyed();
			GameObject.Destroy(mono.gameObject);
		}
	}
}