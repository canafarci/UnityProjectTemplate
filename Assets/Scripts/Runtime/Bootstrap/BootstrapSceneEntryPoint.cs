using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using ProjectTemplate.Runtime.CrossScene.Data;
using ProjectTemplate.Runtime.CrossScene.Scenes.Enums;
using ProjectTemplate.Runtime.CrossScene.Signals;
using ProjectTemplate.Runtime.Infrastructure.ApplicationState;
using ProjectTemplate.Runtime.Infrastructure.ApplicationState.Signals;
using ProjectTemplate.Runtime.Infrastructure.Data;
using ProjectTemplate.Runtime.Infrastructure.Signals;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;

namespace ProjectTemplate.Runtime.Bootstrap
{
	public class BootstrapSceneEntryPoint : IInitializable
	{
		[Inject] private SignalBus _signalBus;
		[Inject] private IGameplayPersistentData _gameplayPersistentData;
		[Inject] private ApplicationSettings _applicationSettings;
		[Inject] private AppInitializer _appInitializer;
		
		public void Initialize()
		{
			_signalBus.Fire(new ChangeAppStateSignal(AppStateID.Initializing));
			_appInitializer.ApplyAppSettings();
			LoadNextScene();		
		}
		
		private void LoadNextScene()
		{
// #if UNITY_EDITOR
// 			if (LoadSceneAfterBootstrap()) return;
// #endif
			
			SceneID sceneID = _applicationSettings.HasMainMenu ?
								SceneID.MainMenu :
								SceneID.Gameplay;
			
			_signalBus.Fire(new LoadSceneSignal(sceneID));
		}

#if UNITY_EDITOR
		private bool LoadSceneAfterBootstrap()
		{
			const string SCENE_TO_LOAD_AFTER_BOOTSTRAP_KEY = "SceneToLoadAfterBootstrap";
			if (UnityEditor.EditorPrefs.HasKey(SCENE_TO_LOAD_AFTER_BOOTSTRAP_KEY))
			{
				string sceneToLoadPath = UnityEditor.EditorPrefs.GetString(SCENE_TO_LOAD_AFTER_BOOTSTRAP_KEY);
				// Clean up the key after use
				UnityEditor.EditorPrefs.DeleteKey(SCENE_TO_LOAD_AFTER_BOOTSTRAP_KEY);

				if (!string.IsNullOrEmpty(sceneToLoadPath))
				{
					// Load the scene by path
					SceneManager.LoadScene(sceneToLoadPath, LoadSceneMode.Single);
					return true;
				}
			}

			return false;
		}
#endif
	}
}