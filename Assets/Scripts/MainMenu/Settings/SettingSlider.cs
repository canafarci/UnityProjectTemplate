using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenu.Settings
{
	public class SettingSlider : MonoBehaviour
	{
		[SerializeField] private Slider Slider;
		[SerializeField] private GameObject OnText;
		[SerializeField] private GameObject OffText;
		
		public async UniTask UpdateAppearance(bool isOn)
		{
			float endValue = isOn ? 1f : 0f;
			OnText.SetActive(isOn);
			OffText.SetActive(!isOn);
			await DOTween.To(() => Slider.value, x => Slider.value = x, endValue, 0.06f);
		}
	}
}