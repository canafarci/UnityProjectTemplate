using System.Collections;
using ProjectTemplate.Runtime.Infrastructure.ApplicationState.Signals;
using ProjectTemplate.Runtime.Infrastructure.MemoryPool;
using ProjectTemplate.Runtime.Infrastructure.MemoryPool.Data;
using ProjectTemplate.Runtime.Infrastructure.Signals;
using ProjectTemplate.Tests.Performance.TestClasses;
using Unity.PerformanceTesting;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using VContainer;
using VContainer.Unity;

namespace ProjectTemplate.Tests.Performance.PoolTests
{
	public class PoolManagerPerformanceTests
	{
		private PoolManager _poolManager;
		private ObjectPool<TestPoolable> _unityPool;
		private Transform _poolParent;
		private GameObject _prefab;

		[UnitySetUp]
		public IEnumerator Setup()
		{
			yield return EditorSceneManager.LoadSceneAsyncInPlayMode("Assets/Scripts/Tests/TestScene.unity", new LoadSceneParameters(LoadSceneMode.Single));

			ContainerBuilder builder = new();
			PoolConfig config = AssetDatabase.LoadAssetAtPath<PoolConfig>("Assets/Scripts/Tests/Test Pool Config.asset");
			Debug.Log(config);
			
			builder.RegisterEntryPoint<PoolManager>().WithParameter(config).AsSelf();
			builder.DeclareSignal<AppStateChangedSignal>();
			builder.RegisterSignalBus();
			IObjectResolver container = builder.Build();
			_poolManager = container.Resolve<PoolManager>();

			PoolEntry entry = config.PoolEntries.Find(x => x.classType == typeof(TestPoolable));
			_prefab = entry.Prefab;
			_poolParent = new GameObject("Pool Parent").transform;
			GameObject.DontDestroyOnLoad(_poolParent);
			
			_unityPool = new ObjectPool<TestPoolable>(CreateObject,
			                  GetFromPool,
			                  ReturnToPool,
			                  DestroyObject,
			                  true,
			                  entry.DefaultCapacity,
			                  entry.MaximumSize);
			
			yield return null;

		}
		
		[UnityTest, Performance]
		public IEnumerator PoolManagerPerformanceTests_PoolManager_OnlyGet()
		{
			Measure.Method(() => { _poolManager.GetMono<TestPoolable>();}) 
				.WarmupCount(100)
				.MeasurementCount(500)
				.Run();
			
			yield return null;
		}
		
		[UnityTest, Performance]
		public IEnumerator PoolManagerPerformanceTests_BasePool_OnlyGet()
		{
			
			Measure.Method(() => { _unityPool.Get();}) 
				.WarmupCount(100)
				.MeasurementCount(500)
				.Run();
			
			yield return null;
		}
		
		[UnityTest, Performance]
		public IEnumerator PoolManagerPerformanceTests_PoolManager_Get_And_Release()
		{
			Measure.Method(() =>
				{
					TestPoolable obj = _poolManager.GetMono<TestPoolable>();
					_poolManager.ReleaseMono(obj);
				}) 
				.WarmupCount(100)
				.MeasurementCount(500)
				.Run();
			
			yield return null;
		}
		
		[UnityTest, Performance]
		public IEnumerator PoolManagerPerformanceTests_BasePool_Get_And_Release()
		{
			
			Measure.Method(() =>
				{
					TestPoolable obj = _unityPool.Get();
					_unityPool.Release(obj);
				}) 
				.WarmupCount(100)
				.MeasurementCount(500)
				.Run();
			
			yield return null;
		}

		
		private TestPoolable CreateObject()
		{
			TestPoolable mono = GameObject.Instantiate(_prefab).GetComponent<TestPoolable>();
			mono.OnCreated();
			
				mono.transform.SetParent(_poolParent);
			
			return mono;
		}

		private  void GetFromPool(TestPoolable mono)
		{
			mono.OnGetFromPool();
			mono.gameObject.SetActive(true);
		}

		private  void ReturnToPool(TestPoolable mono)
		{
			mono.gameObject.SetActive(false);
			
			mono.transform.SetParent(_poolParent);
			
			mono.OnReturnToPool();
		}

		private  void DestroyObject(TestPoolable mono)
		{
			mono.OnDestroyed();
			GameObject.Destroy(mono.gameObject);
		}
	}
}