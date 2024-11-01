using UnityEngine.SceneManagement;

using System;
using DG.Tweening;
using ProjectTemplate.CrossScene.Signals;
using ProjectTemplate.Infrastructure.Templates;

namespace ProjectTemplate.CrossScene
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