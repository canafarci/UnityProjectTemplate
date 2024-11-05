using System.Collections.Generic;
using ProjectTemplate.Runtime.CrossScene.Enums;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectTemplate.Runtime.Gameplay.UI.GameSceneSettings
{
    public class GameSceneSettingsView : SerializedMonoBehaviour
    {
        [SerializeField] private Button TogglePanelButton;
        [SerializeField] private Button HapticButton;
        [SerializeField] private Button SoundButton;
        [SerializeField] private Button ReloadButton;
        [SerializeField] private Button MusicButton;
        [SerializeField] private Image BackgroundImage;
        [SerializeField] private List<Transform> ButtonTransforms;
        [SerializeField] private Dictionary<SettingType, GameObject> SettingsOffGameObjects;

        public Button togglePanelButton => TogglePanelButton;
        public Button hapticButton => HapticButton;
        public Button soundButton => SoundButton;
        public Button musicButton => MusicButton;
        public Button reloadButton => ReloadButton;
        public Dictionary<SettingType, GameObject> settingsOffGameObjects => SettingsOffGameObjects;
        public List<Transform> buttonTransforms => ButtonTransforms;
        public Image backgroundImage => BackgroundImage;
    }
}