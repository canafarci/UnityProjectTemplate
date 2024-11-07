using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using ProjectTemplate.Runtime.CrossScene.Signals;
using ProjectTemplate.Runtime.Infrastructure.ApplicationState;
using ProjectTemplate.Runtime.Infrastructure.ApplicationState.Signals;
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

		private async void OnLoadSceneMessage(LoadSceneSignal signal)
		{
			_signalBus.Fire(new ChangeAppStateSignal(AppStateID.Loading));
			
			DOTween.KillAll();
			GC.Collect();
			await SceneManager.LoadSceneAsync(signal.sceneID);
		}
	}
}