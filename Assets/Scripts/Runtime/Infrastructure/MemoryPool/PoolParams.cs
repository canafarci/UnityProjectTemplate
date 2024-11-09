using ProjectTemplate.Runtime.Infrastructure.ApplicationState;

namespace ProjectTemplate.Runtime.Infrastructure.MemoryPool
{
	public struct PoolParams
	{
		public int InitialSize;
		public int DefaultCapacity;
		public int MaximumSize;
		public bool DontRecycleWithSceneChange;
		public AppStateID RecycleSceneID;

		public PoolParams(int initialSize, 
		                  int defaultCapacity, 
		                  int maximumSize, 
		                  bool dontRecycleWithSceneChange, 
		                  AppStateID recycleSceneID)
		{
			InitialSize = initialSize;
			DefaultCapacity = defaultCapacity;
			MaximumSize = maximumSize;
			DontRecycleWithSceneChange = dontRecycleWithSceneChange;
			RecycleSceneID = recycleSceneID;
		}
	}
}