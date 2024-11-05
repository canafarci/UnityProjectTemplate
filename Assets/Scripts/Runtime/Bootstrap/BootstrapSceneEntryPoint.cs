using DG.Tweening;
using ProjectTemplate.Runtime.CrossScene.Data;
using ProjectTemplate.Runtime.CrossScene.Signals;
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
		public void Initialize()
		{
			InitDOTween();

			LoadNextScene();
		}

		private void InitDOTween()
		{
			DOTween.Init(false, false).SetCapacity(200, 10);
			DOTween.defaultEaseType = Ease.Linear;
		}

		private void LoadNextScene()
		{
#if UNITY_EDITOR
			if (LoadSceneAfterBootstrap()) return;
#endif

			int sceneIndex = _gameplayPersistentData.levelToLoadIndex;
			// Load the default scene
			_signalBus.Fire(new LoadSceneSignal(sceneIndex));
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