using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace ProjectTemplate.Runtime.CrossScene.Signals
{
	public readonly struct LoadingStartedSignal
	{
		private readonly AsyncOperationHandle<SceneInstance> _asyncOperation;

		public AsyncOperationHandle<SceneInstance> asyncOperation => _asyncOperation;
		
		public LoadingStartedSignal(AsyncOperationHandle<SceneInstance> asyncOperation)
		{
			_asyncOperation = asyncOperation;
		}

	}
}