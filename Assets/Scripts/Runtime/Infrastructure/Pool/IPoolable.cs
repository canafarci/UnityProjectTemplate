namespace ProjectTemplate.Runtime.Infrastructure.Pool
{
	public interface IPoolable
	{
		public void OnCreate();
		public void OnDestroy();
		public void OnReturnToPool();
		public void OnTakeFromPool();
	}
}