using ProjectTemplate.Runtime.Infrastructure.Data;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using VContainer;

namespace ProjectTemplate.Runtime.CrossScene.Data
{
	public class GameplayPersistentData : IGameplayPersistentData
	{
		private readonly ApplicationSettings _applicationSettings;
		private readonly AddressableReferences _addressableReferences;

		private int _levelToLoadIndex;
		private int _levelVisualDisplayNumber = ES3.Load(LEVEL_VISUAL_NUMBER, PERSISTENT_DATA_PATH, 1);
		private bool _isFirstTimePlaying;
		
		private	const string LEVEL_TO_LOAD_INDEX = "LEVEL_TO_LOAD_INDEX";
		private	const string LEVEL_VISUAL_NUMBER = "LEVEL_VISUAL_NUMBER";
		private	const string PERSISTENT_DATA_PATH = "PERSISTENT_DATA";
		private	const string IS_FIRST_TIME_PLAYING = "IS_FIRST_TIME_PLAYING";

		public int levelToLoadIndex 
		{
			get { return _levelToLoadIndex; }
#if UNITY_EDITOR
			set { _levelToLoadIndex = value; }
#endif			
		}		
		
		public int levelVisualDisplayNumber => _levelVisualDisplayNumber;

		public GameplayPersistentData(ApplicationSettings applicationSettings, AddressableReferences addressableReferences)
		{
			_applicationSettings = applicationSettings;
			_addressableReferences = addressableReferences;
			_levelToLoadIndex = ES3.Load(LEVEL_TO_LOAD_INDEX, PERSISTENT_DATA_PATH, 0);
		}

		public bool IsFirstTimePlaying()
		{
			_isFirstTimePlaying = ES3.Load(IS_FIRST_TIME_PLAYING, PERSISTENT_DATA_PATH, true);
			ES3.Save(IS_FIRST_TIME_PLAYING, false, PERSISTENT_DATA_PATH);
			return _isFirstTimePlaying;
		}

		public void IncreaseTargetSceneIndex()
		{
			int targetSceneIndex;
			int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
				
			int sceneCount = _addressableReferences.gameplayScenes.Count;
			if (currentSceneIndex + 1 >= sceneCount)
			{
				AssetReference levelToLoop = _addressableReferences.levelToLoopAfterLevelsFinished;
				targetSceneIndex = _addressableReferences.gameplayScenes.IndexOf(levelToLoop);
			}
			else
			{
				targetSceneIndex = currentSceneIndex + 1;
			}

			SaveIndices(targetSceneIndex);
		}

		private void SaveIndices(int targetSceneIndex)
		{
			_levelToLoadIndex = targetSceneIndex;
			ES3.Save(LEVEL_TO_LOAD_INDEX, _levelToLoadIndex, PERSISTENT_DATA_PATH);
			//use this index to fake new level numbers after built in levels end
			_levelVisualDisplayNumber = levelVisualDisplayNumber + 1;
			ES3.Save(LEVEL_VISUAL_NUMBER, levelVisualDisplayNumber, PERSISTENT_DATA_PATH);
		}
	}
}
