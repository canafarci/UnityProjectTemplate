using ProjectTemplate.Runtime.Infrastructure.MemoryPool;
using UnityEngine;

namespace ProjectTemplate.Runtime
{
	public class TestMono : MonoBehaviour, IPoolable
	{
		public void OnCreate()
		{
			Debug.Log("Create");
		}

		public void OnDestroy()
		{
			Debug.Log("Destroy");
		}

		public void OnReturnToPool()
		{
			Debug.Log("Return");
		}

		public void OnGetFromPool()
		{
			Debug.Log("Get");
		}
	}
}