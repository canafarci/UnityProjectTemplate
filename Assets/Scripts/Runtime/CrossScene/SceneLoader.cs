using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using ProjectTemplate.Runtime.CrossScene.LoadingScreen.Signals;
using ProjectTemplate.Runtime.CrossScene.Signals;
using ProjectTemplate.Runtime.Infrastructure.ApplicationState;
using ProjectTemplate.Runtime.Infrastructure.ApplicationState.Signals;
using ProjectTemplate.Runtime.Infrastructure.Data;
using ProjectTemplate.Runtime.Infrastructure.Templates;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;

namespace ProjectTemplate.Runtime.CrossScene
{
	public class SceneLoader : SignalListener
	{
		[Inject] private ApplicationSettings _applicationSettings;
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
			
			await UniTask.NextFrame();
			
			AsyncOperation operation = SceneManager.LoadSceneAsync(signal.sceneID);
			if (_applicationSettings.ShowLoadingScreen)
			{
				_signalBus.Fire(new LoadingStartedSignal(operation));
			}
			
			DOTween.KillAll();
		}
	}
}