using UnityEngine;

using VContainer;
using DG.Tweening;

using ProjectTemplate.Runtime.Infrastructure.Data;

namespace ProjectTemplate.Runtime.Bootstrap
{
	public class AppInitializer
	{
		[Inject] private ApplicationSettings _applicationSettings;

		public void ApplyAppSettings()
		{
			Application.targetFrameRate = _applicationSettings.TargetFrameRate;
			InitDOTween();
		}
		
		private void InitDOTween()
		{
			DOTween.Init(_applicationSettings.RecycleAllByDefault, _applicationSettings.UseSafeMode).
			        SetCapacity(_applicationSettings.TweenCapacity, _applicationSettings.SequenceCapacity);
			
			DOTween.defaultEaseType = _applicationSettings.DefaultEase;
		}
	}
}