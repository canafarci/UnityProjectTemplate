using ProjectTemplate.Runtime.Infrastructure.ApplicationState;

namespace ProjectTemplate.Runtime.Infrastructure.MemoryPool
{
	public struct PoolParams
	{
		public int InitialSize;
		public int DefaultCapacity;
		public int MaximumSize;
		public bool ManagePoolOnSceneChange;
		public AppStateID LifetimeSceneID;

		public PoolParams(int initialSize, 
		                  int defaultCapacity, 
		                  int maximumSize, 
		                  bool managePoolOnSceneChange, 
		                  AppStateID lifetimeSceneID)
		{
			InitialSize = initialSize;
			DefaultCapacity = defaultCapacity;
			MaximumSize = maximumSize;
			ManagePoolOnSceneChange = managePoolOnSceneChange;
			LifetimeSceneID = lifetimeSceneID;
		}
	}
}