using UnityEngine;

namespace ProjectTemplate.Runtime.CrossScene.LoadingScreen.Signals
{
	public readonly struct LoadingStartedSignal
	{
		private readonly AsyncOperation _asyncOperation;

		public AsyncOperation asyncOperation => _asyncOperation;
		
		public LoadingStartedSignal(AsyncOperation asyncOperation)
		{
			_asyncOperation = asyncOperation;
		}

	}
}