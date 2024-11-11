using System;
using System.Collections.Generic;

namespace ProjectTemplate.Runtime.Infrastructure.MemoryPool
{
	public class GenericPool<T> : PoolBase<T> where T : class, IPoolable
	{
		// ReSharper disable once CollectionNeverQueried.Local
		private List<T> _referenceToStopGCList; //needed to stop GC from collecting pure C# classes

		public GenericPool(PoolParams poolParams) : base(poolParams)
		{
			if (!_managePoolOnSceneChange)
			{
				InstantiateDefaultObjects();
			}
		}

		protected override T CreateObject()
		{
			T obj = Activator.CreateInstance<T>();
			obj.OnCreated();
			
			//needed to stop GC from collecting pure C# classes
			if (!_managePoolOnSceneChange)
			{
				_referenceToStopGCList ??= new List<T>();
				_referenceToStopGCList.Add(obj);
			}
			
			return obj;
		}

		protected override void GetFromPool(T obj) => obj.OnGetFromPool();

		protected override void ReturnToPool(T obj) => obj.OnReturnToPool();

		protected override void DestroyObject(T obj) => obj.OnDestroyed();
	}
}