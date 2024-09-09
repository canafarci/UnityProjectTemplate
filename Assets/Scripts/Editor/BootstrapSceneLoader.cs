using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace ProjectTemplate.Editor
{
	[InitializeOnLoad]
	public class BootstrapSceneLoader
	{
		private const string PREVIOUS_SCENE_KEY = "PreviousScene";
		private const string SHOULD_LOAD_STARTUP_SCENE_KEY = "LoadStartupScene";

		private const string LOAD_STARTUP_SCENE_ON_PLAY = "Development/Load Startup Scene On Play";
		private const string DONT_LOAD_STARTUP_SCENE_ON_PLAY = "Development/Don't Load Startup Scene On Play";

		private static bool _restartingToSwitchedScene;

		private static string bootstrapScene => EditorBuildSettings.scenes[0].path;

		static BootstrapSceneLoader()
		{
			EditorApplication.playModeStateChanged += EditorApplicationOnPlayModeStateChanged;
		}

		#region Getters-Setters

		private static string previousScene
		{
			get => EditorPrefs.GetString(PREVIOUS_SCENE_KEY);
			set => EditorPrefs.SetString(PREVIOUS_SCENE_KEY, value);
		}

		private static bool shouldLoadStartupScene
		{
			get
			{
				if (!EditorPrefs.HasKey(SHOULD_LOAD_STARTUP_SCENE_KEY))
					EditorPrefs.SetBool(SHOULD_LOAD_STARTUP_SCENE_KEY, true);

				return EditorPrefs.GetBool(SHOULD_LOAD_STARTUP_SCENE_KEY);
			}

			set => EditorPrefs.SetBool(SHOULD_LOAD_STARTUP_SCENE_KEY, value);
		}

		#endregion

		[MenuItem(LOAD_STARTUP_SCENE_ON_PLAY, true)]
		private static bool ShowLoadBootstrapSceneOnPlay() => !shouldLoadStartupScene;

		[MenuItem(LOAD_STARTUP_SCENE_ON_PLAY)]
		private static void EnableLoadBootstrapSceneOnPlay()
		{
			shouldLoadStartupScene = true;
		}

		[MenuItem(DONT_LOAD_STARTUP_SCENE_ON_PLAY, true)]
		private static bool ShowDoNotLoadBootstrapSceneOnPlay() => shouldLoadStartupScene;

		[MenuItem(DONT_LOAD_STARTUP_SCENE_ON_PLAY)]
		private static void DisableDoNotLoadBootstrapSceneOnPlay()
		{
			shouldLoadStartupScene = false;
		}

		private static void EditorApplicationOnPlayModeStateChanged(PlayModeStateChange playModeStateChange)
		{
			if (!shouldLoadStartupScene) return;

			if (_restartingToSwitchedScene) //error check as multiple starts and stops happening
			{
				if (playModeStateChange == PlayModeStateChange.EnteredPlayMode) _restartingToSwitchedScene = false;
				return;
			}

			if (playModeStateChange == PlayModeStateChange.ExitingEditMode)
			{
				// cache previous scene to return to it after play session ends
				previousScene = EditorSceneManager.GetActiveScene().path;

				if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
				{
					// user either hit "Save" or "Don't Save"; open bootstrap scene

					if (!string.IsNullOrEmpty(bootstrapScene) &&
					    System.Array.Exists(EditorBuildSettings.scenes, scene => scene.path == bootstrapScene))
					{
						Scene activeScene = EditorSceneManager.GetActiveScene();

						_restartingToSwitchedScene =
							activeScene.path == string.Empty || !bootstrapScene.Contains(activeScene.path);

						// only switch if editor is in a empty scene or active scene is not startup scene
						if (_restartingToSwitchedScene)
						{
							EditorApplication.isPlaying = false;

							// scene is included in build settings; open it
							EditorSceneManager.OpenScene(bootstrapScene);

							EditorApplication.isPlaying = true;
						}
					}
				}
				else
				{
					// user either hit "Cancel" or exited window; don't open startup scene & return to editor
					EditorApplication.isPlaying = false;
				}
			}
			//return to last open scene
			else if (playModeStateChange == PlayModeStateChange.EnteredEditMode)
			{
				if (!string.IsNullOrEmpty(previousScene)) EditorSceneManager.OpenScene(previousScene);
			}
		}
	}
}