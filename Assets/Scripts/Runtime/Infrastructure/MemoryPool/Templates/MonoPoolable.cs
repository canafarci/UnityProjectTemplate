using UnityEngine;

namespace ProjectTemplate.Runtime.Infrastructure.MemoryPool.Templates
{
	public abstract class MonoPoolable : MonoBehaviour, IPoolable
	{
		public virtual void OnCreated()
		{
		}

		public virtual void OnDestroyed()
		{
		}

		public virtual void OnReturnToPool()
		{
		}

		public virtual void OnGetFromPool()
		{
		}
	}
}