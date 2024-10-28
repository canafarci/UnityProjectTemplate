// using System.Collections.Generic;
// using Sirenix.OdinInspector;
// using UnityEngine;
// using UnityEngine.UI;
//
// namespace ProjectTemplate.Gameplay.GameSceneSettings
// {
//     public class GameSceneSettingsView : SerializedMonoBehaviour
//     {
//         [SerializeField] private Button _togglePanelButton;
//         [SerializeField] private Button _exitButton;
//         [SerializeField] private Button _soundButton;
//         [SerializeField] private Button _musicButton;
//         [SerializeField] private Button _backgroundButton;
//         [SerializeField] private GameObject _backgroundImage;
//         [SerializeField] private List<Transform> _buttonTransforms;
//         [SerializeField] private Dictionary<SettingsTypes, GameObject> _settingsOffGameObjects;
//
//         public Button TogglePanelButton => _togglePanelButton;
//         public Button ExitButton => _exitButton;
//         public Button SoundButton => _soundButton;
//         public Button MusicButton => _musicButton;
//         public Dictionary<SettingsTypes, GameObject> SettingsOffGameObjects => _settingsOffGameObjects;
//         public List<Transform> ButtonTransforms => _buttonTransforms;
//         public GameObject BackgroundImage => _backgroundImage;
//         public Button BackgroundButton => _backgroundButton;
//     }
// }