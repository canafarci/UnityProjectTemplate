using UnityEngine.SceneManagement;

using System;
using DG.Tweening;
using VContainer;
using VContainer.Unity;

using ProjectTemplate.CrossScene.Signals;
using ProjectTemplate.Infrastructure.SignalBus;

namespace ProjectTemplate.CrossScene
{
	public class SceneLoader : IInitializable, IDisposable
	{
		[Inject] SignalBus _signalBus;
		
		public void Initialize()
		{
			_signalBus.Subscribe<LoadSceneSignal>(OnLoadSceneMessage);
		}

		private void OnLoadSceneMessage(LoadSceneSignal signal)
		{
			DOTween.KillAll();
			GC.Collect();
			SceneManager.LoadSceneAsync(signal.sceneID);
		}

		public void Dispose()
		{
			_signalBus.Unsubscribe<LoadSceneSignal>(OnLoadSceneMessage);
		}
	}
}