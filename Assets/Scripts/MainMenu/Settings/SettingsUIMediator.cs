using System;
using MainMenu.Enums;
using UnityEngine;
using VContainer.Unity;

namespace MainMenu.Settings
{
	public class SettingsUIMediator : IInitializable, IDisposable
	{
		private readonly SettingsUIView _view;
		public event Action<SettingButton> OnToggleSettingButtonClicked; 

		public SettingsUIMediator(SettingsUIView view)
		{
			_view = view;
		}
		
		public async void ChangeSliderVisual(SettingButton button, bool isOn)
		{
			switch (button)
			{
				case SettingButton.Music:
					_view.toggleMusicButton.interactable = false;
					await _view.musicSettingSlider.UpdateAppearance(isOn);
					_view.toggleMusicButton.interactable = true;
					break;
				case SettingButton.Sound:
					_view.toggleSoundButton.interactable = false;
					await _view.soundSettingSlider.UpdateAppearance(isOn);
					_view.toggleSoundButton.interactable = true;
					break;
				case SettingButton.Haptic:
					_view.toggleHapticButton.interactable = false;
					await _view.hapticSettingSlider.UpdateAppearance(isOn);
					_view.toggleHapticButton.interactable = true;
					break;
				case SettingButton.Notification:
					_view.toggleNotificationsButton.interactable = false;
					await _view.notificationSettingSlider.UpdateAppearance(isOn);
					_view.toggleNotificationsButton.interactable = true;
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(button), button, null);
			}
		}
		
		public void Initialize()
		{
			_view.privacyPolicyButton.onClick.AddListener(OnPrivacyPolicyButtonClicked);
			_view.facebookDataDeleteButton.onClick.AddListener(OnFacebookDataDeleteButtonClicked);
			_view.termsAndConditionsButton.onClick.AddListener(OnTermsButtonClicked);
			_view.restorePurchasesButton.onClick.AddListener(OnRestorePurchasesClicked);
			_view.toggleMusicButton.onClick.AddListener(() => OnToggleSettingButtonClicked?.Invoke(SettingButton.Music));
			_view.toggleSoundButton.onClick.AddListener(() => OnToggleSettingButtonClicked?.Invoke(SettingButton.Sound));
			_view.toggleHapticButton.onClick.AddListener(() => OnToggleSettingButtonClicked?.Invoke(SettingButton.Haptic));
			_view.toggleNotificationsButton.onClick.AddListener(() => OnToggleSettingButtonClicked?.Invoke(SettingButton.Notification));
			_view.backButton.onClick.AddListener(OnBackButtonClicked);
			_view.openSettingsButton.onClick.AddListener(OnOpenSettingsClicked);
		}

		private void OnOpenSettingsClicked()
		{
			_view.gameObject.SetActive(true);
			_view.openSettingsButton.gameObject.SetActive(false);
		}

		private void OnBackButtonClicked()
		{
			_view.gameObject.SetActive(false);
			_view.openSettingsButton.gameObject.SetActive(true);
		}

		private void OnFacebookDataDeleteButtonClicked()
		{
			Application.OpenURL("https://www.teekgames.com/facebook-data-deletion-instructions/");
		}

		private void OnPrivacyPolicyButtonClicked()
		{
			Application.OpenURL("https://www.teekgames.com/privacy-policy/");
		}

		private void OnTermsButtonClicked()
		{
			Application.OpenURL("https://www.teekgames.com/terms-and-conditions/");
		}

		private void OnRestorePurchasesClicked()
		{
		}

		public void Dispose()
		{
			_view.privacyPolicyButton.onClick.RemoveAllListeners();
			_view.facebookDataDeleteButton.onClick.RemoveAllListeners();
			_view.termsAndConditionsButton.onClick.RemoveAllListeners();
			_view.restorePurchasesButton.onClick.RemoveAllListeners();
			_view.toggleMusicButton.onClick.RemoveAllListeners();
			_view.toggleSoundButton.onClick.RemoveAllListeners();
			_view.toggleHapticButton.onClick.RemoveAllListeners();
			_view.toggleNotificationsButton.onClick.RemoveAllListeners();
			_view.backButton.onClick.RemoveAllListeners();
		}
	}
}