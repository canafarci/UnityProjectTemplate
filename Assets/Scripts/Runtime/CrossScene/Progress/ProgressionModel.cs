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
		public int progressCountToUnlock { get; }
		public bool allElementsUnlocked { get; }
		public int progressIndex { get; }
		public Sprite lastUnlockedElementIcon { get; }
		public string lastUnlockedElementName { get; }
		public int lastUnlockedThreshold { get; }
	}

	public class ProgressionModel : IProgressModel
	{
		private readonly ProgressionData _data;
		private HashSet<UnlockableID> _unlockedElements;
		private int _progressIndex;
		private int _unlockableElementIndex;

		// new fields:
		private Sprite _lastUnlockedIcon;
		private string _lastUnlockedName;
		private int _lastUnlockedThreshold;

		private const string UNLOCKED_ELEMENTS_LOOKUP_KEY = "UNLOCKED_ELEMENTS_LOOKUP_KEY";
		private const string PROGRESS_INDEX_KEY = "PROGRESS_INDEX_KEY";
		private const string UNLOCKABLE_ELEMENT_INDEX_KEY = "UNLOCKABLE_ELEMENT_INDEX_KEY";
		private const string PROGRESS_DATA_PATH = "PROGRESS_DATA_PATH";

		// always clamp so we never go out of range
		private int CurrentStepIndex => Mathf.Clamp(_unlockableElementIndex, 0, _data.progressSteps.Count - 1);

		public ProgressionModel(ProgressionData data)
		{
			_data = data;
			_unlockedElements = ES3.Load(UNLOCKED_ELEMENTS_LOOKUP_KEY,
			                             PROGRESS_DATA_PATH,
			                             new HashSet<UnlockableID>());
			_progressIndex = ES3.Load(PROGRESS_INDEX_KEY,
			                          PROGRESS_DATA_PATH,
			                          0);
			_unlockableElementIndex = ES3.Load(UNLOCKABLE_ELEMENT_INDEX_KEY,
			                                   PROGRESS_DATA_PATH,
			                                   0);

			// initialize last-unlocked to the most recently unlocked (if any)
			if (_unlockedElements.Count > 0)
			{
				// find highest index in data.progressSteps whose ID is in unlockedElements
				for (int i = _data.progressSteps.Count - 1; i >= 0; i--)
				{
					if (_unlockedElements.Contains(_data.progressSteps[i].UnlockableID))
					{
						_lastUnlockedIcon = _data.progressSteps[i].Icon;
						_lastUnlockedName = _data.progressSteps[i].Name;
						_lastUnlockedThreshold = _data.progressSteps[i].LevelsToCompleteToUnlock;
						break;
					}
				}
			}
		}

		// interface props for “next” step
		public Sprite progressElementIcon => _data.progressSteps[CurrentStepIndex].Icon;
		public string progressElementName => _data.progressSteps[CurrentStepIndex].Name;
		public int progressCountToUnlock => _data.progressSteps[CurrentStepIndex].LevelsToCompleteToUnlock;

		public bool allElementsUnlocked => _unlockedElements.Count >= _data.progressSteps.Count;
		public int progressIndex => _progressIndex;

		// interface props for “just-unlocked” step
		public Sprite lastUnlockedElementIcon => _lastUnlockedIcon;
		public string lastUnlockedElementName => _lastUnlockedName;
		public int lastUnlockedThreshold => _lastUnlockedThreshold;

		public bool IsUnlocked(UnlockableID id) => _unlockedElements.Contains(id);

		public void IncrementProgress(out bool unlocked)
		{
			_progressIndex++;

			int idx = CurrentStepIndex;
			int targetCount = _data.progressSteps[idx].LevelsToCompleteToUnlock;

			if (_progressIndex == targetCount)
			{
				unlocked = true;

				// capture the just-unlocked step BEFORE bumping the index
				_lastUnlockedIcon = _data.progressSteps[idx].Icon;
				_lastUnlockedName = _data.progressSteps[idx].Name;
				_lastUnlockedThreshold = targetCount;

				_unlockedElements.Add(_data.progressSteps[idx].UnlockableID);

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
			ES3.Save(PROGRESS_INDEX_KEY, _progressIndex, PROGRESS_DATA_PATH);
			ES3.Save(UNLOCKABLE_ELEMENT_INDEX_KEY, _unlockableElementIndex, PROGRESS_DATA_PATH);
		}
	}
}