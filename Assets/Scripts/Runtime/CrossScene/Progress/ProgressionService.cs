using System;

namespace ProjectTemplate.Runtime.CrossScene.Progress
{
	public class ProgressionService
	{
		private readonly IProgressModel _model;
		private readonly ProgressionData _data;
		
		// Fired when progress increments but <b>not</b> fully unlocked
		public event Action<int /* newIndex */, int /* targetCount */> OnProgressIncreased;

		// Fired when the current step just unlocked
		public event Action<int /* targetCount */> OnUnlockedAll;



		public ProgressionService(IProgressModel model, ProgressionData data)
		{
			_model = model;
			_data = data;
		}

		// Call after a level-win to bump progress
		public void HandleLevelWin()
		{
			if (_data.progressSteps == null || _data.progressSteps.Count == 0) return;
			
			_model.IncrementProgress(out bool unlocked);

			if (unlocked)
				OnUnlockedAll?.Invoke(_model.progressCountToUnlock);
			else
				OnProgressIncreased?.Invoke(_model.progressIndex, _model.progressCountToUnlock);
		}
	}
}