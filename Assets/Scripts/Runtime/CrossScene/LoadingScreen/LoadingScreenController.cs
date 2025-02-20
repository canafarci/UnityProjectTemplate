using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using ProjectTemplate.Runtime.CrossScene.Signals;
using ProjectTemplate.Runtime.Infrastructure.Data;
using ProjectTemplate.Runtime.Infrastructure.Templates;
using UnityEngine;
using VContainer;

namespace ProjectTemplate.Runtime.CrossScene.LoadingScreen
{
	public class LoadingScreenController : SignalListener
	{
		private readonly ApplicationSettings _applicationSettings;
		private readonly LoadingScreenView _view;

		public LoadingScreenController(ApplicationSettings applicationSettings, LoadingScreenView view)
		{
			_applicationSettings = applicationSettings;
			_view = view;
		}
		
		protected override void SubscribeToEvents()
		{
			_signalBus.Subscribe<LoadingStartedSignal>(OnLoadingStartedSignal);
		}

		private async void OnLoadingStartedSignal(LoadingStartedSignal signal)
		{
			float startTime = Time.realtimeSinceStartup;
			
			_view.gameObject.SetActive(true);
			_view.fillImage.fillAmount = 0f;
			
			while (!signal.asyncOperation.IsDone)
			{
				LerpFillImage(signal.asyncOperation.PercentComplete);
				await UniTask.NextFrame();
			}

			while (startTime + _applicationSettings.LoadingScreenMinimumDuration > Time.realtimeSinceStartup)
			{
				LerpFillImage(1f);
				await UniTask.NextFrame();
			}
			
			_view.gameObject.SetActive(false);
			_signalBus.Fire(new LoadingFinishedSignal());
		}

		private void LerpFillImage(float targetFillAmount)
		{
			_view.fillImage.fillAmount = Mathf.Lerp(_view.fillImage.fillAmount, targetFillAmount, Time.deltaTime * 10f);
		}

		protected override void UnsubscribeFromEvents()
		{
			_signalBus.Unsubscribe<LoadingStartedSignal>(OnLoadingStartedSignal);
		}
	}
}