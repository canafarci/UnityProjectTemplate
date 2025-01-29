using System.Collections.Generic;

namespace ProjectTemplate.Runtime.CrossScene.Progress
{
	public class ProgressService
	{
		private readonly IProgressModel _model;
		private readonly ProgressData _data;

		public ProgressService(IProgressModel model, ProgressData data)
		{
			_model = model;
			_data = data;
		}
	}
}