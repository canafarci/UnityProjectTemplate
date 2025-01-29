using UnityEngine;

namespace ProjectTemplate.Runtime.Infrastructure.MemoryPool
{
	public class GenericMonoPool<T> : PoolBase<T> where T : MonoBehaviour, IPoolable
	{
		private readonly GameObject _prefab;
		private readonly Transform _poolParent;

		public GenericMonoPool(GameObject prefab, Transform poolParent, PoolParams poolParams) : base(poolParams)
		{
			_prefab = prefab;
			_poolParent = poolParent;

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
				mono.transform.SetParent(_poolParent);
			
			return mono;
		}

		protected override void GetFromPool(T mono)
		{
			mono.OnGetFromPool();
			mono.gameObject.SetActive(true);
		}

		protected override void ReturnToPool(T mono)
		{
			mono.gameObject.SetActive(false);
			
			if (!_managePoolOnSceneChange)
				mono.transform.SetParent(_poolParent);
			
			mono.OnReturnToPool();
		}

		protected override void DestroyObject(T mono)
		{
			mono.OnDestroyed();
			GameObject.Destroy(mono.gameObject);
		}
	}
}