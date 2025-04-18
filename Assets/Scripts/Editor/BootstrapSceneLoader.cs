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

        // Flag to prevent recursive scene switching.
        private static bool _restartingToSwitchedScene;

        // The bootstrap scene must be the first scene in Build Settings.
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

        /// <summary>
        /// Stores the previous scene's GUID.
        /// </summary>
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

        #region Menu Items

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
        private static void DisableDoNotLoadBootstrapSceneOnPlay()
        {
            loadSceneMode = LoadSceneMode.DoNotLoadStartupScene;
        }

        [MenuItem(DONT_LOAD_BOOTSTRAP_SCENE_ON_PLAY, true)]
        private static bool ValidateDisableDoNotLoadBootstrapSceneOnPlay()
        {
            Menu.SetChecked(DONT_LOAD_BOOTSTRAP_SCENE_ON_PLAY, loadSceneMode == LoadSceneMode.DoNotLoadStartupScene);
            return true;
        }

        [MenuItem(LOAD_BOOTSTRAP_AND_CURRENT_SCENE_ON_PLAY)]
        private static void EnableLoadBootstrapAndCurrentSceneOnPlay()
        {
            loadSceneMode = LoadSceneMode.LoadStartupAndCurrentScene;
        }

        [MenuItem(LOAD_BOOTSTRAP_AND_CURRENT_SCENE_ON_PLAY, true)]
        private static bool ValidateEnableLoadBootstrapAndCurrentSceneOnPlay()
        {
            Menu.SetChecked(LOAD_BOOTSTRAP_AND_CURRENT_SCENE_ON_PLAY, loadSceneMode == LoadSceneMode.LoadStartupAndCurrentScene);
            return true;
        }

        #endregion

        private static void EditorApplicationOnPlayModeStateChanged(PlayModeStateChange playModeStateChange)
        {
            // Skip bootstrap behavior if a test is running or if the mode is set to do nothing.
            if (EditorPrefs.HasKey(IS_TESTING_KEY) || loadSceneMode == LoadSceneMode.DoNotLoadStartupScene)
                return;

            if (_restartingToSwitchedScene)
            {
                if (playModeStateChange == PlayModeStateChange.EnteredPlayMode)
                    _restartingToSwitchedScene = false;
                return;
            }

            if (playModeStateChange == PlayModeStateChange.ExitingEditMode)
            {
                // Capture the active scene's GUID (instead of its path).
                Scene activeScene = EditorSceneManager.GetActiveScene();
                string activeSceneGUID = AssetDatabase.AssetPathToGUID(activeScene.path);
                previousScene = activeSceneGUID;

                if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                {
                    // Ensure the bootstrap scene is valid and part of Build Settings.
                    if (!string.IsNullOrEmpty(bootstrapScene) &&
                        System.Array.Exists(EditorBuildSettings.scenes, scene => scene.path == bootstrapScene))
                    {
                        // Only switch if the active scene is not already the bootstrap scene.
                        _restartingToSwitchedScene = string.IsNullOrEmpty(activeScene.path) || !bootstrapScene.Contains(activeScene.path);

                        if (_restartingToSwitchedScene)
                        {
                            EditorApplication.isPlaying = false;

                            // If the "Load Bootstrap then Current Scene" option is active, store the active scene’s GUID.
                            if (loadSceneMode == LoadSceneMode.LoadStartupAndCurrentScene)
                            {
                                EditorPrefs.SetString(SCENE_TO_LOAD_AFTER_BOOTSTRAP_KEY, previousScene);
                            }
                            else
                            {
                                // Clear any previous setting.
                                EditorPrefs.DeleteKey(SCENE_TO_LOAD_AFTER_BOOTSTRAP_KEY);
                            }

                            // Open the bootstrap scene.
                            EditorSceneManager.OpenScene(bootstrapScene);
                            EditorApplication.isPlaying = true;
                        }
                    }
                }
                else
                {
                    // User cancelled the save operation.
                    EditorApplication.isPlaying = false;
                }
            }
            else if (playModeStateChange == PlayModeStateChange.EnteredEditMode)
            {
                // Clean up stored keys after exiting Play mode.
                EditorPrefs.DeleteKey(SCENE_TO_LOAD_AFTER_BOOTSTRAP_KEY);

                // Restore the previously open scene (using its GUID).
                if (!string.IsNullOrEmpty(previousScene))
                {
                    string scenePath = AssetDatabase.GUIDToAssetPath(previousScene);
                    if (!string.IsNullOrEmpty(scenePath))
                    {
                        EditorSceneManager.OpenScene(scenePath);
                    }
                }
            }
        }
    }
}