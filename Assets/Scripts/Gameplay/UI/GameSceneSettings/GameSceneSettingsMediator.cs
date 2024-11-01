using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using ProjectTemplate.CrossScene.Enums;
using ProjectTemplate.Gameplay.Enums;
using ProjectTemplate.Utility.Extensions;
using VContainer.Unity;

namespace ProjectTemplate.Gameplay.UI.GameSceneSettings
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

            switch (lastActivationState)
            {
                case PanelActivationState.Inactive:
                {
                    _view.backgroundImage.gameObject.SetActive(true);
                    _view.backgroundImage.AnimateBackgroundAlpha(0f, 0.9f, .2f);

                    for (int i = 0; i < buttonTransformsCount; i++)
                        await _view.buttonTransforms[i].transform.DOLocalMoveX(ON_X_POS, 0.1f).SetEase(Ease.OutBack);
                    break;
                }
                    case PanelActivationState.Active:
                {
                    _view.backgroundImage.gameObject.SetActive(false);

                    for (int i = buttonTransformsCount - 1; i >= 0; i--)
                        await _view.buttonTransforms[i].transform.DOLocalMoveX(OFF_X_POS, 0.1f).SetEase(Ease.InBack);;
                    break;
                }
                default:
                    break;
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