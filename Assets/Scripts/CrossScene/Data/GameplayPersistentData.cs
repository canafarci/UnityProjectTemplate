using UnityEngine.SceneManagement;

namespace ProjectTemplate.CrossScene.Data
{
	public class GameplayPersistentData
	{
		private int _levelToLoadIndex = ES3.Load(LEVEL_TO_LOAD_INDEX, PERSISTENT_DATA_PATH, 2);
		private int _levelVisualDisplayNumber = ES3.Load(LEVEL_VISUAL_NUMBER, PERSISTENT_DATA_PATH, 1);
		private int _towerHealthUpgradeLevel;
		private bool _isFirstTimePlaying;
		
		private	const string LEVEL_TO_LOAD_INDEX = "LEVEL_TO_LOAD_INDEX";
		private	const string LEVEL_VISUAL_NUMBER = "LEVEL_VISUAL_NUMBER";
		private	const string PERSISTENT_DATA_PATH = "PERSISTENT_DATA";
		private	const string IS_FIRST_TIME_PLAYING = "IS_FIRST_TIME_PLAYING";
		private const int LOOPING_LEVEL_INDEX = 2;

		public int levelToLoadIndex => _levelToLoadIndex;
		public int levelVisualDisplayNumber => _levelVisualDisplayNumber;

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
				
			int sceneCount = SceneManager.sceneCountInBuildSettings;
			if (currentSceneIndex + 1 == sceneCount)
			{
				targetSceneIndex = LOOPING_LEVEL_INDEX;
			}
			else
			{
				targetSceneIndex = currentSceneIndex + 1;
			}

			_levelToLoadIndex = targetSceneIndex;
			ES3.Save(LEVEL_TO_LOAD_INDEX, _levelToLoadIndex, PERSISTENT_DATA_PATH);
			//use this index to fake new level numbers after built in levels end
			_levelVisualDisplayNumber = levelVisualDisplayNumber + 1;
			ES3.Save(LEVEL_VISUAL_NUMBER, levelVisualDisplayNumber, PERSISTENT_DATA_PATH);
		}
	}
}