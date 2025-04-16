using Cysharp.Threading.Tasks;
using DG.Tweening;
using ProjectTemplate.Runtime.CrossScene.Data;
using ProjectTemplate.Runtime.CrossScene.Scenes.Enums;
using ProjectTemplate.Runtime.CrossScene.Signals;
using ProjectTemplate.Runtime.Infrastructure.ApplicationState;
using ProjectTemplate.Runtime.Infrastructure.ApplicationState.Signals;
using ProjectTemplate.Runtime.Infrastructure.Data;
using ProjectTemplate.Runtime.Infrastructure.Templates;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace ProjectTemplate.Runtime.CrossScene.Scenes
{
	public class SceneLoader : SignalListener
	{
		private readonly ApplicationSettings _applicationSettings;
		private readonly AddressableReferences _addressableReferences;
		private readonly IGameplayPersistentData _gameplayPersistentData;
		
		private AsyncOperationHandle<SceneInstance> _handle;

		public SceneLoader(ApplicationSettings applicationSettings,
			AddressableReferences addressableReferences,
			IGameplayPersistentData gameplayPersistentData)
		{
			_applicationSettings = applicationSettings;
			_addressableReferences = addressableReferences;
			_gameplayPersistentData = gameplayPersistentData;
		}

		protected override void SubscribeToEvents()
		{
			_signalBus.Subscribe<LoadSceneSignal>(OnLoadSceneMessage);
		}

		protected override void UnsubscribeFromEvents()
		{
			_signalBus.Unsubscribe<LoadSceneSignal>(OnLoadSceneMessage);
		}

		private async void OnLoadSceneMessage(LoadSceneSignal signal)
		{
			_signalBus.Fire(new ChangeAppStateSignal(AppStateID.Loading));
			
			if (_handle.IsValid())
			{
				await Addressables.UnloadSceneAsync(_handle);
			}
			
			if (signal.sceneID == SceneID.MainMenu)
			{
				AssetReference sceneReference = _addressableReferences.mainMenuScene;
				_handle = Addressables.LoadSceneAsync(sceneReference, LoadSceneMode.Additive);
			}
			else
			{
				int sceneIndex = _gameplayPersistentData.levelToLoadIndex;
				AssetReference sceneReference = _addressableReferences.gameplayScenes[sceneIndex];
				_handle = Addressables.LoadSceneAsync(sceneReference, LoadSceneMode.Additive);
			}
			
			_handle.Completed += OnLoadingComplete;
			
			if (_applicationSettings.ShowLoadingScreen)
			{
				_signalBus.Fire(new LoadingStartedSignal(_handle));
			}
			
			DOTween.KillAll();
		}

		private void OnLoadingComplete(AsyncOperationHandle<SceneInstance> handle)
		{
			HandleSceneLoadComplete(handle).Forget();
			handle.Completed -= OnLoadingComplete;
		}

		private async UniTaskVoid HandleSceneLoadComplete(AsyncOperationHandle<SceneInstance> handle)
		{
			await handle.Result.ActivateAsync();
			SceneManager.SetActiveScene(handle.Result.Scene);
			_signalBus.Fire(new SceneActivatedSignal());
		}
	}
}