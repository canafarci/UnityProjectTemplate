using Cysharp.Threading.Tasks;
using DG.Tweening;
using ProjectTemplate.Runtime.CrossScene.Data;
using ProjectTemplate.Runtime.CrossScene.Scenes.Enums;
using ProjectTemplate.Runtime.CrossScene.Signals;
using ProjectTemplate.Runtime.Infrastructure.ApplicationState;
using ProjectTemplate.Runtime.Infrastructure.ApplicationState.Signals;
using ProjectTemplate.Runtime.Infrastructure.Data;
using ProjectTemplate.Runtime.Infrastructure.Signals;
using UnityEngine;
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
        [Inject] private AddressableReferences _addressableReferences;
        
        public void Initialize()
        {
            _signalBus.Fire(new ChangeAppStateSignal(AppStateID.Initializing));
            _appInitializer.ApplyAppSettings();

#if UNITY_EDITOR
            if (TryLoadEditorScene())
                return;
#endif
            // Fallback: load the default scene (MainMenu or Gameplay).
            SceneID sceneID = _applicationSettings.HasMainMenu ? SceneID.MainMenu : SceneID.Gameplay;
            _signalBus.Fire(new LoadSceneSignal(sceneID));
        }
        
#if UNITY_EDITOR
        /// If a scene was stored by the bootstrap editor loader, try to resolve it and trigger its loading.
        private bool TryLoadEditorScene()
        {
            const string key = "SceneToLoadAfterBootstrap";
            if (UnityEditor.EditorPrefs.HasKey(key))
            {
                // Retrieve and clean up the stored scene GUID.
                string sceneGUID = UnityEditor.EditorPrefs.GetString(key);
                UnityEditor.EditorPrefs.DeleteKey(key);
                if (string.IsNullOrEmpty(sceneGUID)) return false;
                
                // Convert GUID to scene path.
                string scenePath = UnityEditor.AssetDatabase.GUIDToAssetPath(sceneGUID);
                
                if (string.IsNullOrEmpty(scenePath)) return false;
                
                // Use the AddressableReferences to find the matching AssetReference.
                int index = _addressableReferences.gameplayScenes.FindIndex(refAsset =>
                {
                    // In the editor, refAsset.editorAsset gives access to the actual asset.
                    string assetPath = UnityEditor.AssetDatabase.GetAssetPath(refAsset.editorAsset);
                    return assetPath == scenePath;
                });

                if (index < 0) return false;
                
                // Set the persistent level index so that your Addressables‑based SceneLoader loads the correct scene.
                _gameplayPersistentData.levelToLoadIndex = index;
                // Fire the signal that triggers loading via Addressables.
                _signalBus.Fire(new LoadSceneSignal(SceneID.Gameplay));
                return true;
            }
            return false;
        }
#endif
    }
}
