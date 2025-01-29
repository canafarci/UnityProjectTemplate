using System.Collections.Generic;
using ProjectTemplate.Runtime.CrossScene.Enums;
using UnityEngine;

namespace ProjectTemplate.Runtime.CrossScene.Progress
{
	public interface IProgressModel
	{
		public bool IsUnlocked(UnlockableID unlockableID);
		public void IncrementProgress(out bool unlocked);
		public Sprite progressElementIcon { get; }
		public string progressElementName { get; }
		public bool allElementsUnlocked { get; }
		public int progressIndex { get; }
		public int progressCountToUnlock { get; }

	}
	
	public class ProgressModel : IProgressModel
	{
		private readonly ProgressData _data;
		private HashSet<UnlockableID> _unlockedElements;
		private int _progressIndex;
		private int _unlockableElementIndex;
		
		private const string UNLOCKED_ELEMENTS_LOOKUP_KEY = "UNLOCKED_ELEMENTS_LOOKUP_KEY";
		private const string PROGRESS_INDEX_KEY = "PROGRESS_INDEX_KEY";
		private const string UNLOCKABLE_ELEMENT_INDEX_KEY = "UNLOCKABLE_ELEMENT_INDEX_KEY";
		private const string PROGRESS_DATA_PATH = "PROGRESS_DATA_PATH";

		public Sprite progressElementIcon => _data.progressSteps[_unlockableElementIndex].Icon;
		public string progressElementName => _data.progressSteps[_unlockableElementIndex].Name;
		public bool allElementsUnlocked => _unlockedElements.Count >= _data.progressSteps.Count;
		public int progressIndex => _progressIndex;
		public int progressCountToUnlock => _data.progressSteps[_unlockableElementIndex].LevelsToCompleteToUnlock;



		public ProgressModel(ProgressData data)
		{
			_data = data;
			_unlockedElements = ES3.Load(UNLOCKED_ELEMENTS_LOOKUP_KEY, PROGRESS_DATA_PATH, new HashSet<UnlockableID>());
			_progressIndex = ES3.Load(PROGRESS_INDEX_KEY, PROGRESS_DATA_PATH, 0);
			_unlockableElementIndex = ES3.Load(UNLOCKABLE_ELEMENT_INDEX_KEY, PROGRESS_DATA_PATH, 0);
		}
		
		public bool IsUnlocked(UnlockableID unlockableID)
		{
			return _unlockedElements.Contains(unlockableID);
		}

		public void IncrementProgress(out bool unlocked)
		{
			_progressIndex = progressIndex + 1;

			if (progressIndex == _data.progressSteps[_unlockableElementIndex].LevelsToCompleteToUnlock)
			{
				unlocked = true;
				_unlockedElements.Add(_data.progressSteps[_unlockableElementIndex].UnlockableID);
				_unlockableElementIndex++;
				_progressIndex = 0;
			}
			else
			{
				unlocked = false;
			}

			SaveState();
		}


		private void SaveState()
		{
			ES3.Save(UNLOCKED_ELEMENTS_LOOKUP_KEY, _unlockedElements, PROGRESS_DATA_PATH);
			ES3.Save(PROGRESS_INDEX_KEY, progressIndex, PROGRESS_DATA_PATH);
			ES3.Save(UNLOCKABLE_ELEMENT_INDEX_KEY, _unlockableElementIndex, PROGRESS_DATA_PATH);
		}
	}
}