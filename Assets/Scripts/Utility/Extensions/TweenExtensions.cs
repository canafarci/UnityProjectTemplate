using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectTemplate.Utility.Extensions
{
	public static class TweenExtensions
	{
		public static void AnimateBackgroundAlpha(this Image image, float startValue, float endValue, float duration)
		{
			Color color = image.color;
			color.a = startValue;
			image.color = color;
			
			DOTween.ToAlpha(()=> image.color, x=> image.color = x, endValue, duration);
		}	
	}
}