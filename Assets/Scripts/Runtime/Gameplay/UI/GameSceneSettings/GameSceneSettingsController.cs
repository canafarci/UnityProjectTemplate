using System;
using ProjectTemplate.Runtime.CrossScene.Audio;
using ProjectTemplate.Runtime.CrossScene.Enums;
using ProjectTemplate.Runtime.CrossScene.Haptic;
using ProjectTemplate.Runtime.CrossScene.Signals;
using ProjectTemplate.Runtime.Gameplay.Enums;
using ProjectTemplate.Runtime.Infrastructure.Signals;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;

namespace ProjectTemplate.Runtime.Gameplay.UI.GameSceneSettings
{
    public class GameSceneSettingsController : IInitializable, IDisposable
    {
        [Inject] private SignalBus _signalBus;
        
        private readonly GameSceneSettingsMediator _gameSceneSettingsMediator;
        private readonly IAudioModel _audioModel;
        private readonly IHapticModel _hapticModel;

        private PanelActivationState _activationState = PanelActivationState.Inactive;
        
        public GameSceneSettingsController(GameSceneSettingsMediator gameSceneSettingsMediator,
                                           IAudioModel audioModel,
                                           IHapticModel hapticModel)
        {
            _gameSceneSettingsMediator = gameSceneSettingsMediator;
            _audioModel = audioModel;
            _hapticModel = hapticModel;
        }

        public void Initialize()
        {
            _gameSceneSettingsMediator.OnGameSettingsChanged += GameSettingsChangedHandler;
            
            _gameSceneSettingsMediator.SetUpOnOffIcons(SettingType.Music, _audioModel.isMusicEnabled);
            _gameSceneSettingsMediator.SetUpOnOffIcons(SettingType.Sound, _audioModel.isSoundEnabled);
            _gameSceneSettingsMediator.SetUpOnOffIcons(SettingType.Haptic, _hapticModel.isEnabled);
        }

        private void GameSettingsChangedHandler(GameSceneSettingsOption gameSceneSettingsOption)
        {
            switch (gameSceneSettingsOption)
            {
                case GameSceneSettingsOption.Reload:
                    OnReloadButtonClicked();
                    break;
                case GameSceneSettingsOption.Music:
                    OnMusicButtonClicked();
                    break;
                case GameSceneSettingsOption.Sound:
                    OnSoundButtonClicked();
                    break;
                case GameSceneSettingsOption.Haptic:
                    OnHapticButtonClicked();
                    break;
                case GameSceneSettingsOption.ToggleActivation:
                    OnToggleButtonClicked();
                    break;
                default:
                    throw new Exception("No settings option has been found!");
            }
        }

        private void OnHapticButtonClicked()
        {
            _gameSceneSettingsMediator.SetUpOnOffIcons(SettingType.Haptic, !_hapticModel.isEnabled);
            _signalBus.Fire(new ChangeHapticActivationSignal());
        }

        private void OnReloadButtonClicked()
        {
            int sceneIndex = SceneManager.GetActiveScene().buildIndex;
            _signalBus.Fire(new LoadSceneSignal(sceneIndex));
        }
        
        private void OnMusicButtonClicked()
        {
            _gameSceneSettingsMediator.SetUpOnOffIcons(SettingType.Music, !_audioModel.isMusicEnabled);
            _signalBus.Fire(new ChangeAudioSettingsSignal(AudioSourceType.Music));
        }
        
        private void OnSoundButtonClicked()
        {
            _gameSceneSettingsMediator.SetUpOnOffIcons(SettingType.Sound, !_audioModel.isSoundEnabled);
            _signalBus.Fire(new ChangeAudioSettingsSignal(AudioSourceType.SoundEffect));
        }
        
        private async void OnToggleButtonClicked()
        {
            if (_activationState == PanelActivationState.Animating) return;
            PanelActivationState lastActivationState = _activationState;
            
            _activationState = PanelActivationState.Animating;
            
             await _gameSceneSettingsMediator.AnimateButtons(lastActivationState);

             _activationState = lastActivationState == PanelActivationState.Active
                                    ? PanelActivationState.Inactive
                                    : PanelActivationState.Active;
        }

        public void Dispose()
        {
            _gameSceneSettingsMediator.OnGameSettingsChanged -= GameSettingsChangedHandler;
        }
    }
}