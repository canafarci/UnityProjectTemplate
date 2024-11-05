using System;
using DG.Tweening;
using ProjectTemplate.Runtime.CrossScene.Signals;
using ProjectTemplate.Runtime.Infrastructure.Templates;
using UnityEngine.SceneManagement;

namespace ProjectTemplate.Runtime.CrossScene
{
	public class SceneLoader : SignalListener
	{
		protected override void SubscribeToEvents()
		{
			_signalBus.Subscribe<LoadSceneSignal>(OnLoadSceneMessage);
		}

		protected override void UnsubscribeFromEvents()
		{
			_signalBus.Unsubscribe<LoadSceneSignal>(OnLoadSceneMessage);
		}

		private void OnLoadSceneMessage(LoadSceneSignal signal)
		{
			DOTween.KillAll();
			GC.Collect();
			SceneManager.LoadSceneAsync(signal.sceneID);
		}
	}
}