// using System;
// using Cysharp.Threading.Tasks;
// using DG.Tweening;
// using VContainer.Unity;
//
// namespace ProjectTemplate.Gameplay.GameSceneSettings
// {
//     public class GameSceneSettingsMediator : IInitializable, IDisposable
//     {
//         private readonly GameSceneSettingsView _gameSceneSettingsView;
//
//         public event Action<GameSceneSettingsOption> OnGameSettingsChanged; 
//
//         public GameSceneSettingsMediator( GameSceneSettingsView gameSceneSettingsView)
//         {
//             _gameSceneSettingsView = gameSceneSettingsView;
//         }
//
//         public void Initialize()
//         {
//             _gameSceneSettingsView.ExitButton.onClick.AddListener(() => FireGameSceneSettingsChangeSignal(GameSceneSettingsOption.Exit));
//             _gameSceneSettingsView.MusicButton.onClick.AddListener(() => FireGameSceneSettingsChangeSignal(GameSceneSettingsOption.Music));
//             _gameSceneSettingsView.SoundButton.onClick.AddListener(() => FireGameSceneSettingsChangeSignal(GameSceneSettingsOption.Sound));
//             _gameSceneSettingsView.TogglePanelButton.onClick.AddListener(() => FireGameSceneSettingsChangeSignal(GameSceneSettingsOption.ToggleActivation));
//             _gameSceneSettingsView.BackgroundButton.onClick.AddListener(() => FireGameSceneSettingsChangeSignal(GameSceneSettingsOption.ToggleActivation));
//         }
//
//         private void FireGameSceneSettingsChangeSignal(GameSceneSettingsOption gameSceneSettingsOption)
//         {
//             OnGameSettingsChanged?.Invoke(gameSceneSettingsOption);
//         }
//         
//         public void SetUpOnOffIcons(SettingsTypes settingsType, bool isMuted)
//         {
//             _gameSceneSettingsView.SettingsOffGameObjects[settingsType].SetActive(isMuted);
//         }
//         
//         public void Dispose()
//         {
//             _gameSceneSettingsView.ExitButton.onClick.RemoveAllListeners();
//             _gameSceneSettingsView.MusicButton.onClick.RemoveAllListeners();
//             _gameSceneSettingsView.SoundButton.onClick.RemoveAllListeners();
//             _gameSceneSettingsView.TogglePanelButton.onClick.RemoveAllListeners();
//         }
//
//         public async UniTask AnimateButtons(PanelActivationState lastActivationState)
//         {
//             int buttonTransformsCount = _gameSceneSettingsView.ButtonTransforms.Count;
//             
//             if (lastActivationState == PanelActivationState.Inactive)
//             {
//                 _gameSceneSettingsView.BackgroundImage.SetActive(true);
//                 
//                 for (int i = 0; i < buttonTransformsCount; i++)
//                    await _gameSceneSettingsView.ButtonTransforms[i].transform.DOLocalMoveX(-50, 0.1f);
//             }
//             else
//             {
//                 _gameSceneSettingsView.BackgroundImage.SetActive(false);
//
//                 for (int i = buttonTransformsCount - 1; i >= 0; i--)
//                     await _gameSceneSettingsView.ButtonTransforms[i].transform.DOLocalMoveX(200, 0.1f);
//             }
//         }
//     }
// }