using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenu.Settings
{
    public class SettingsUIView : SerializedMonoBehaviour
    {
        [SerializeField] private Button FacebookDataDeleteButton;
        [SerializeField] private Button RestorePurchasesButton;
        [SerializeField] private Button PrivacyPolicyButton;
        [SerializeField] private Button TermsAndConditionsButton;
        [SerializeField] private Button BackButton;
        [SerializeField] private Button OpenSettingsButton;
        [SerializeField] private Button ToggleMusicButton;
        [SerializeField] private Button ToggleSoundButton;
        [SerializeField] private Button ToggleHapticButton;
        [SerializeField] private Button ToggleNotificationsButton;
        [SerializeField] private SettingSlider MusicSettingSlider;
        [SerializeField] private SettingSlider SoundSettingSlider;
        [SerializeField] private SettingSlider NotificationSettingSlider;
        [SerializeField] private SettingSlider HapticSettingSlider;
        
        public Button facebookDataDeleteButton => FacebookDataDeleteButton;

        public Button restorePurchasesButton => RestorePurchasesButton;

        public Button privacyPolicyButton => PrivacyPolicyButton;

        public Button termsAndConditionsButton => TermsAndConditionsButton;

        public Button backButton => BackButton;

        public Button toggleMusicButton => ToggleMusicButton;

        public Button toggleSoundButton => ToggleSoundButton;

        public Button toggleHapticButton => ToggleHapticButton;

        public Button toggleNotificationsButton => ToggleNotificationsButton;

        public SettingSlider musicSettingSlider => MusicSettingSlider;

        public SettingSlider soundSettingSlider => SoundSettingSlider;

        public SettingSlider notificationSettingSlider => NotificationSettingSlider;

        public SettingSlider hapticSettingSlider => HapticSettingSlider;

        public Button openSettingsButton => OpenSettingsButton;
    }
}