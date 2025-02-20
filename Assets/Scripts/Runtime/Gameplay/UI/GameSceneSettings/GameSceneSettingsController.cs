using System;
using ProjectTemplate.Runtime.CrossScene.Audio;
using ProjectTemplate.Runtime.CrossScene.Enums;
using ProjectTemplate.Runtime.CrossScene.Haptic;
using ProjectTemplate.Runtime.CrossScene.Signals;
using ProjectTemplate.Runtime.Gameplay.Enums;
using ProjectTemplate.Runtime.Gameplay.Signals;
using ProjectTemplate.Runtime.Infrastructure.Signals;
using Lofelt.NiceVibrations;
using Cysharp.Threading.Tasks;
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
                    OnToggleButtonClicked().Forget();
                    break;
                default:
                    throw new Exception($"No settings option has been found for {gameSceneSettingsOption}!");
            }
        }

        private void OnHapticButtonClicked()
        {
            _gameSceneSettingsMediator.SetUpOnOffIcons(SettingType.Haptic, !_hapticModel.isEnabled);
            _signalBus.Fire(new ChangeHapticActivationSignal());
            
            _signalBus.Fire(new PlayHapticSignal(HapticPatterns.PresetType.MediumImpact));
        }

        private void OnReloadButtonClicked()
        {
            _signalBus.Fire(new PlayHapticSignal(HapticPatterns.PresetType.MediumImpact));
            
            _signalBus.Fire(new ChangeGameStateSignal(GameState.Reloading));
            _signalBus.Fire(new TriggerExitGameplayLevelSignal(reload: true));
        }
        
        private void OnMusicButtonClicked()
        {
            _signalBus.Fire(new PlayHapticSignal(HapticPatterns.PresetType.MediumImpact));

            _gameSceneSettingsMediator.SetUpOnOffIcons(SettingType.Music, !_audioModel.isMusicEnabled);
            _signalBus.Fire(new ChangeAudioSettingsSignal(AudioSourceType.Music));
        }
        
        private void OnSoundButtonClicked()
        {
            _signalBus.Fire(new PlayHapticSignal(HapticPatterns.PresetType.MediumImpact));

            _gameSceneSettingsMediator.SetUpOnOffIcons(SettingType.Sound, !_audioModel.isSoundEnabled);
            _signalBus.Fire(new ChangeAudioSettingsSignal(AudioSourceType.SoundEffect));
        }
        
        private async UniTask OnToggleButtonClicked()
        {
            _signalBus.Fire(new PlayHapticSignal(HapticPatterns.PresetType.MediumImpact));

            if (_activationState != PanelActivationState.Animating)
            {
                HandlePreToggleState();
        
                PanelActivationState previousState = _activationState;
                _activationState = PanelActivationState.Animating;
        
                await _gameSceneSettingsMediator.AnimateButtons(previousState);
        
                UpdatePostToggleState(previousState);
            }
        }

        private void HandlePreToggleState()
        {
            if (_activationState == PanelActivationState.Inactive)
            {
                _signalBus.Fire(new ChangeGameStateSignal(GameState.Paused));
            }
        }

        private void UpdatePostToggleState(PanelActivationState previousState)
        {
            _activationState = previousState == PanelActivationState.Inactive
                                   ? PanelActivationState.Active
                                   : PanelActivationState.Inactive;

            if (_activationState == PanelActivationState.Inactive)
            {
                _signalBus.Fire(new ChangeGameStateSignal(GameState.Playing));
            }
        }

        public void Dispose()
        {
            _gameSceneSettingsMediator.OnGameSettingsChanged -= GameSettingsChangedHandler;
        }
    }
}