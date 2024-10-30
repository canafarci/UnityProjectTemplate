using System;

using Cysharp.Threading.Tasks;
using DG.Tweening;
using VContainer.Unity;

using ProjectTemplate.CrossScene.Enums;
using ProjectTemplate.Gameplay.Enums;

namespace ProjectTemplate.Gameplay.GameSceneSettings
{
    public class GameSceneSettingsMediator : IInitializable, IDisposable
    {
        private readonly GameSceneSettingsView _view;
        private const float OFF_X_POS = 740f;
        private const float ON_X_POS = 486f;
        public event Action<GameSceneSettingsOption> OnGameSettingsChanged; 

        public GameSceneSettingsMediator( GameSceneSettingsView view)
        {
            _view = view;
        }

        public void Initialize()
        {
            _view.hapticButton.onClick.AddListener(() => OnGameSettingsChanged?.Invoke(GameSceneSettingsOption.Haptic));
            _view.musicButton.onClick.AddListener(() => OnGameSettingsChanged?.Invoke(GameSceneSettingsOption.Music));
            _view.soundButton.onClick.AddListener(() => OnGameSettingsChanged?.Invoke(GameSceneSettingsOption.Sound));
            _view.togglePanelButton.onClick.AddListener(() => OnGameSettingsChanged?.Invoke(GameSceneSettingsOption.ToggleActivation));
            _view.reloadButton.onClick.AddListener(() => OnGameSettingsChanged?.Invoke(GameSceneSettingsOption.Reload));
        }
        
        public void SetUpOnOffIcons(SettingType settingsType, bool isEnabled)
        {
            _view.settingsOffGameObjects[settingsType].SetActive(!isEnabled);
        }

        public async UniTask AnimateButtons(PanelActivationState lastActivationState)
        {
            int buttonTransformsCount = _view.buttonTransforms.Count;
            
            if (lastActivationState == PanelActivationState.Inactive)
            {
                _view.backgroundImage.SetActive(true);
                
                for (int i = 0; i < buttonTransformsCount; i++)
                   await _view.buttonTransforms[i].transform.DOLocalMoveX(ON_X_POS, 0.1f);
            }
            else
            {
                _view.backgroundImage.SetActive(false);

                for (int i = buttonTransformsCount - 1; i >= 0; i--)
                    await _view.buttonTransforms[i].transform.DOLocalMoveX(OFF_X_POS, 0.1f);
            }
        }
        
        public void Dispose()
        {
            _view.hapticButton.onClick.RemoveAllListeners();
            _view.musicButton.onClick.RemoveAllListeners();
            _view.soundButton.onClick.RemoveAllListeners();
            _view.togglePanelButton.onClick.RemoveAllListeners();
            _view.reloadButton.onClick.RemoveAllListeners();
        }
    }
}