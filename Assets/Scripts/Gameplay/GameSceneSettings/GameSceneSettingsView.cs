using UnityEngine;
using UnityEngine.UI;

using System.Collections.Generic;

using Sirenix.OdinInspector;

using ProjectTemplate.CrossScene.Enums;
using UnityEngine.Serialization;

namespace ProjectTemplate.Gameplay.GameSceneSettings
{
    public class GameSceneSettingsView : SerializedMonoBehaviour
    {
        [SerializeField] private Button TogglePanelButton;
        [SerializeField] private Button HapticButton;
        [SerializeField] private Button SoundButton;
        [SerializeField] private Button ReloadButton;
        [SerializeField] private Button MusicButton;
        [SerializeField] private GameObject BackgroundImage;
        [SerializeField] private List<Transform> ButtonTransforms;
        [SerializeField] private Dictionary<SettingType, GameObject> SettingsOffGameObjects;

        public Button togglePanelButton => TogglePanelButton;
        public Button hapticButton => HapticButton;
        public Button soundButton => SoundButton;
        public Button musicButton => MusicButton;
        public Button reloadButton => ReloadButton;
        public Dictionary<SettingType, GameObject> settingsOffGameObjects => SettingsOffGameObjects;
        public List<Transform> buttonTransforms => ButtonTransforms;
        public GameObject backgroundImage => BackgroundImage;
    }
}