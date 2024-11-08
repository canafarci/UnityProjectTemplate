namespace ProjectTemplate.Runtime.Infrastructure.MemoryPool
{
	public interface IPoolable
	{
		public void OnCreate();
		public void OnDestroy();
		public void OnReturnToPool();
		public void OnGetFromPool();
	}
}