// using UnityEngine;
//
// using System;
//
// using VContainer;
// using VContainer.Unity;
//
// using ProjectTemplate.CrossScene.Audio;
// using ProjectTemplate.Infrastructure.SignalBus;
//
// namespace ProjectTemplate.Gameplay.GameSceneSettings
// {
//     public class GameSceneSettingsController : IInitializable, IDisposable
//     {
//         [Inject] private SignalBus _signalBus;
//         
//         private readonly GameSceneSettingsMediator _gameSceneSettingsMediator;
//         private readonly IAudioModel _audioModel;
//         
//         private PanelActivationState _activationState = PanelActivationState.Inactive;
//         
//         public GameSceneSettingsController(GameSceneSettingsMediator gameSceneSettingsMediator, IAudioModel audioModel)
//         {
//             _gameSceneSettingsMediator = gameSceneSettingsMediator;
//             _audioModel = audioModel;
//         }
//
//         public void Initialize()
//         {
//             _gameSceneSettingsMediator.OnGameSettingsChanged += GameSettingsChangedHandler;
//             
//             _gameSceneSettingsMediator.SetUpOnOffIcons(SettingsTypes.Music, _audioModel.IsMusicMuted);
//             _gameSceneSettingsMediator.SetUpOnOffIcons(SettingsTypes.Sound, _audioModel.IsSoundMuted);
//         }
//
//         private void GameSettingsChangedHandler(GameSceneSettingsOption gameSceneSettingsOption)
//         {
//             switch (gameSceneSettingsOption)
//             {
//                 case GameSceneSettingsOption.Exit:
//                     OnExitButtonClicked();
//                     break;
//                 case GameSceneSettingsOption.Music:
//                     OnMusicButtonClicked();
//                     break;
//                 case GameSceneSettingsOption.Sound:
//                     OnSoundButtonClicked();
//                     break;
//                 case GameSceneSettingsOption.ToggleActivation:
//                     OnToggleButtonClicked();
//                     break;
//                 default:
//                     throw new Exception("No settings option has been found!");
//             }
//         }
//         
//         private void OnExitButtonClicked()
//         {
//             _loadSceneMessagePublisher.Publish(new LoadSceneMessage(1));
//
//         }
//         
//         private void OnMusicButtonClicked()
//         {
//             _gameSceneSettingsMediator.SetUpOnOffIcons(SettingsTypes.Music, !_audioModel.IsMusicMuted);
//             _changeAudioSettingsPublisher.Publish(new ChangeAudioSettingsMessage(AudioType.Music));
//         }
//         
//         private void OnSoundButtonClicked()
//         {
//             _gameSceneSettingsMediator.SetUpOnOffIcons(SettingsTypes.Sound, !_audioModel.IsSoundMuted);
//             _changeAudioSettingsPublisher.Publish(new ChangeAudioSettingsMessage(AudioType.Sound));
//         }
//         
//         private async void OnToggleButtonClicked()
//         {
//             if (_activationState == PanelActivationState.Animating) return;
//             PanelActivationState lastActivationState = _activationState;
//             
//             _activationState = PanelActivationState.Animating;
//             
//              await _gameSceneSettingsMediator.AnimateButtons(lastActivationState);
//
//              _activationState = lastActivationState == PanelActivationState.Active
//                                     ? PanelActivationState.Inactive
//                                     : PanelActivationState.Active;
//         }
//
//         public void Dispose()
//         {
//             _gameSceneSettingsMediator.OnGameSettingsChanged -= GameSettingsChangedHandler;
//         }
//     }
// }