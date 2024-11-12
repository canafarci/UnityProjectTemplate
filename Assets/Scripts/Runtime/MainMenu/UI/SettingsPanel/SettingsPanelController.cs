using System;
using ProjectTemplate.Runtime.CrossScene.Audio;
using ProjectTemplate.Runtime.CrossScene.Haptic;
using ProjectTemplate.Runtime.CrossScene.Notifications;
using ProjectTemplate.Runtime.MainMenu.Enums;
using VContainer;
using VContainer.Unity;

namespace ProjectTemplate.Runtime.MainMenu.UI.SettingsPanel
{
	public class SettingsPanelController : IInitializable, IDisposable
	{
		[Inject] private IAudioModel _audioModel;
		[Inject] private IHapticModel _hapticModel;
		[Inject] private INotificationModel _notificationModel;
		
		private readonly SettingsPanelMediator _mediator;

		public SettingsPanelController(SettingsPanelMediator mediator)
		{
			_mediator = mediator;
		}
		
		public void Initialize()
		{
			_mediator.OnToggleSettingButtonClicked += ToggleSettingButtonClickedHandler;
			
			_mediator.ChangeSliderVisual(SettingButton.Music, _audioModel.isMusicEnabled);
			_mediator.ChangeSliderVisual(SettingButton.Sound, _audioModel.isSoundEnabled);
			_mediator.ChangeSliderVisual(SettingButton.Haptic, _hapticModel.isEnabled);
			_mediator.ChangeSliderVisual(SettingButton.Notification, _notificationModel.isEnabled);
		}

		private void ToggleSettingButtonClickedHandler(SettingButton button)
		{
			switch (button)
			{
				case SettingButton.Music:
					_audioModel.ChangeMusicActivation();
					_mediator.ChangeSliderVisual(button, _audioModel.isMusicEnabled);
					break;
				case SettingButton.Sound:
					_audioModel.ChangeSoundActivation();
					_mediator.ChangeSliderVisual(button, _audioModel.isSoundEnabled);
					break;
				case SettingButton.Haptic:
					_hapticModel.ChangeHapticActivation();
					_mediator.ChangeSliderVisual(button, _hapticModel.isEnabled);
					break;
				case SettingButton.Notification:
					_notificationModel.ChangeNotificationActivation();
					_mediator.ChangeSliderVisual(button, _notificationModel.isEnabled);
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(button), button, null);
			}
		}

		public void Dispose()
		{
			_mediator.OnToggleSettingButtonClicked -= ToggleSettingButtonClickedHandler;
		}
	}
}