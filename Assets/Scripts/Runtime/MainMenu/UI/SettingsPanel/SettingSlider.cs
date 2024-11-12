using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using DG.Tweening;

namespace ProjectTemplate.Runtime.MainMenu.UI.SettingsPanel
{
	public class SettingSlider : MonoBehaviour
	{
		[SerializeField] private Slider Slider;
		[SerializeField] private GameObject OnText;
		[SerializeField] private GameObject OffText;
		
		public async UniTask UpdateAppearance(bool isEnabled)
		{
			float endValue = isEnabled ? 1f : 0f;
			OnText.SetActive(isEnabled);
			OffText.SetActive(!isEnabled);
			await DOTween.To(() => Slider.value, x => Slider.value = x, endValue, 0.06f);
		}
	}
}