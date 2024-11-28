using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.TestTools.TestRunner.Api;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectTemplate.Editor
{
	[InitializeOnLoad]
	public class BootstrapSceneLoader
	{
		private const string PREVIOUS_SCENE_KEY = "PreviousScene";
		private const string LOAD_SCENE_MODE_KEY = "LoadSceneMode";
		private const string SCENE_TO_LOAD_AFTER_BOOTSTRAP_KEY = "SceneToLoadAfterBootstrap";

		private const string IS_TESTING_KEY = "IsTestingPlayMode";

		private const string LOAD_BOOTSTRAP_SCENE_ON_PLAY = "Tools/Bootstrap Scene Loader/Load Bootstrap Scene On Play";
		private const string DONT_LOAD_BOOTSTRAP_SCENE_ON_PLAY = "Tools/Bootstrap Scene Loader/Don't Load Bootstrap Scene On Play";
		private const string LOAD_BOOTSTRAP_AND_CURRENT_SCENE_ON_PLAY = "Tools/Bootstrap Scene Loader/Load Bootstrap then Current Scene On Play";

		private static bool _restartingToSwitchedScene;

		//bootstrap scene needs to be inside build settings at index 0
		private static string bootstrapScene => EditorBuildSettings.scenes[0].path;

		static BootstrapSceneLoader()
		{
			EditorApplication.playModeStateChanged += EditorApplicationOnPlayModeStateChanged;
		}

		private enum LoadSceneMode
		{
			DoNotLoadStartupScene = 0,
			LoadStartupSceneOnly = 1,
			LoadStartupAndCurrentScene = 2
		}

		#region Getters-Setters

		private static string previousScene
		{
			get => EditorPrefs.GetString(PREVIOUS_SCENE_KEY);
			set => EditorPrefs.SetString(PREVIOUS_SCENE_KEY, value);
		}

		private static LoadSceneMode loadSceneMode
		{
			get
			{
				if (!EditorPrefs.HasKey(LOAD_SCENE_MODE_KEY))
					EditorPrefs.SetInt(LOAD_SCENE_MODE_KEY, (int)LoadSceneMode.LoadStartupSceneOnly);

				return (LoadSceneMode)EditorPrefs.GetInt(LOAD_SCENE_MODE_KEY);
			}
			set => EditorPrefs.SetInt(LOAD_SCENE_MODE_KEY, (int)value);
		}

		#endregion

		[MenuItem(LOAD_BOOTSTRAP_SCENE_ON_PLAY)]
		private static void EnableLoadBootstrapSceneOnPlay()
		{
			loadSceneMode = LoadSceneMode.LoadStartupSceneOnly;
		}

		[MenuItem(LOAD_BOOTSTRAP_SCENE_ON_PLAY, true)]
		private static bool ValidateEnableLoadBootstrapSceneOnPlay()
		{
			Menu.SetChecked(LOAD_BOOTSTRAP_SCENE_ON_PLAY, loadSceneMode == LoadSceneMode.LoadStartupSceneOnly);
			return true;
		}

		[MenuItem(DONT_LOAD_BOOTSTRAP_SCENE_ON_PLAY)]
		private static void DisableDoNotLoadStartupSceneOnPlay()
		{
			loadSceneMode = LoadSceneMode.DoNotLoadStartupScene;
		}

		[MenuItem(DONT_LOAD_BOOTSTRAP_SCENE_ON_PLAY, true)]
		private static bool ValidateDisableDoNotLoadStartupSceneOnPlay()
		{
			Menu.SetChecked(DONT_LOAD_BOOTSTRAP_SCENE_ON_PLAY, loadSceneMode == LoadSceneMode.DoNotLoadStartupScene);
			return true;
		}

		[MenuItem(LOAD_BOOTSTRAP_AND_CURRENT_SCENE_ON_PLAY)]
		private static void EnableLoadStartupAndCurrentSceneOnPlay()
		{
			loadSceneMode = LoadSceneMode.LoadStartupAndCurrentScene;
		}

		[MenuItem(LOAD_BOOTSTRAP_AND_CURRENT_SCENE_ON_PLAY, true)]
		private static bool ValidateEnableLoadStartupAndCurrentSceneOnPlay()
		{
			Menu.SetChecked(LOAD_BOOTSTRAP_AND_CURRENT_SCENE_ON_PLAY, loadSceneMode == LoadSceneMode.LoadStartupAndCurrentScene);
			return true;
		}

		private static void EditorApplicationOnPlayModeStateChanged(PlayModeStateChange playModeStateChange)
		{
			// Skip BootstrapSceneLoader behavior if a test is running
			if (EditorPrefs.HasKey(IS_TESTING_KEY) || loadSceneMode == LoadSceneMode.DoNotLoadStartupScene) return;
			if (_restartingToSwitchedScene)
			{
				if (playModeStateChange == PlayModeStateChange.EnteredPlayMode) _restartingToSwitchedScene = false;
				return;
			}

			if (playModeStateChange == PlayModeStateChange.ExitingEditMode)
			{
				previousScene = EditorSceneManager.GetActiveScene().path;

				if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
				{
					if (!string.IsNullOrEmpty(bootstrapScene) &&
					    System.Array.Exists(EditorBuildSettings.scenes, scene => scene.path == bootstrapScene))
					{
						Scene activeScene = EditorSceneManager.GetActiveScene();

						_restartingToSwitchedScene =
							activeScene.path == string.Empty || !bootstrapScene.Contains(activeScene.path);

						if (_restartingToSwitchedScene)
						{
							EditorApplication.isPlaying = false;

							// Set the scene to load after bootstrap based on the selected mode
							if (loadSceneMode == LoadSceneMode.LoadStartupAndCurrentScene)
							{
								EditorPrefs.SetString(SCENE_TO_LOAD_AFTER_BOOTSTRAP_KEY, previousScene);
							}
							else
							{
								// Clear any previous setting
								EditorPrefs.DeleteKey(SCENE_TO_LOAD_AFTER_BOOTSTRAP_KEY);
							}

							// Open the bootstrap scene
							EditorSceneManager.OpenScene(bootstrapScene);

							EditorApplication.isPlaying = true;
						}
					}
				}
				else
				{
					// User canceled the save operation
					EditorApplication.isPlaying = false;
				}
			}
			else if (playModeStateChange == PlayModeStateChange.EnteredEditMode)
			{
				// Clean up after exiting play mode
				EditorPrefs.DeleteKey(SCENE_TO_LOAD_AFTER_BOOTSTRAP_KEY);

				if (!string.IsNullOrEmpty(previousScene)) EditorSceneManager.OpenScene(previousScene);
			}
		}
	}
}