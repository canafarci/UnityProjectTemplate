using Cysharp.Threading.Tasks;
using ProjectTemplate.Runtime.CrossScene.Signals;
using ProjectTemplate.Runtime.Infrastructure.Data;
using ProjectTemplate.Runtime.Infrastructure.Initialization.Signals;
using ProjectTemplate.Runtime.Infrastructure.Templates;
using UnityEngine;

namespace ProjectTemplate.Runtime.CrossScene.LoadingScreen
{
    public class LoadingScreenController : SignalListener
    {
        private readonly ApplicationSettings _applicationSettings;
        private readonly LoadingScreenView _view;

        // Indicates whether the close signal has been received.
        private bool _isCloseLoadingScreenRequested;

        private const float FILL_ANIMATION_SPEED = 10f;

        public LoadingScreenController(ApplicationSettings applicationSettings, LoadingScreenView view)
        {
            _applicationSettings = applicationSettings;
            _view = view;
        }

        protected override void SubscribeToEvents()
        {
            _signalBus.Subscribe<LoadingStartedSignal>(OnLoadingStartedSignal);
            _signalBus.Subscribe<AllModulesInitializedSignal>(OnAllModulesInitializedSignal);
        }

        protected override void UnsubscribeFromEvents()
        {
            _signalBus.Unsubscribe<LoadingStartedSignal>(OnLoadingStartedSignal);
            _signalBus.Unsubscribe<AllModulesInitializedSignal>(OnAllModulesInitializedSignal);
        }

        // Handles the start of a loading operation by displaying and animating the loading screen.
        // Waits until the async operation is done, the minimum display duration has passed, 
        // and a close signal is received before hiding the screen.
        private async void OnLoadingStartedSignal(LoadingStartedSignal signal)
        {
            ResetLoadingScreenState();
            ActivateLoadingScreen();

            float loadingScreenStartTime = Time.realtimeSinceStartup;

            // Animate progress until the async operation completes.
            while (!signal.asyncOperation.IsDone)
            {
                UpdateFillAmount(signal.asyncOperation.PercentComplete);
                await UniTask.NextFrame();
            }
            
            // Wait until both the minimum duration has passed and the close signal has been received.
            while (!_isCloseLoadingScreenRequested || !HasMinimumDurationPassed(loadingScreenStartTime))
            {
                UpdateFillAmount(1f);
                await UniTask.NextFrame();
            }
            
            DeactivateLoadingScreen();
        }
        
        private void OnAllModulesInitializedSignal(AllModulesInitializedSignal signal) => _isCloseLoadingScreenRequested = true; 
        
        private void ResetLoadingScreenState() => _isCloseLoadingScreenRequested = false;

        private void ActivateLoadingScreen()
        {
            _view.gameObject.SetActive(true);
            _view.fillImage.fillAmount = 0f;
        }
        
        private void DeactivateLoadingScreen() => _view.gameObject.SetActive(false);
        
        private void UpdateFillAmount(float targetFillAmount) => _view.fillImage.fillAmount = Mathf.Lerp(_view.fillImage.fillAmount, targetFillAmount, Time.deltaTime * FILL_ANIMATION_SPEED);
        
        private bool HasMinimumDurationPassed(float startTime) => (Time.realtimeSinceStartup - startTime) >= _applicationSettings.LoadingScreenMinimumDuration;
    }
}
