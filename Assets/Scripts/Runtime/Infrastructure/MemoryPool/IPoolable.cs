namespace ProjectTemplate.Runtime.Infrastructure.MemoryPool
{
	public interface IPoolable
	{
		public void OnCreated();
		public void OnDestroyed();
		public void OnReturnToPool();
		public void OnGetFromPool();
	}
}