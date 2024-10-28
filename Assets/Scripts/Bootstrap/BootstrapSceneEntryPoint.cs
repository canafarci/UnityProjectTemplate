using ProjectTemplate.CrossScene.Enums;
using UnityEngine.SceneManagement;

using VContainer;
using VContainer.Unity;

using ProjectTemplate.CrossScene.Signals;
using ProjectTemplate.Infrastructure.SignalBus;

namespace ProjectTemplate.Bootstrap
{
	public class BootstrapSceneEntryPoint : IInitializable
	{
		[Inject] private SignalBus _signalBus;
		public void Initialize()
		{
#if UNITY_EDITOR
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
					return;
				}
			}
#endif

			int defaultSceneIndex = 1; // Default scene index
			// Load the default scene
			_signalBus.Fire(new LoadSceneSignal(defaultSceneIndex)); // Publish load scene message
		}
	}
}