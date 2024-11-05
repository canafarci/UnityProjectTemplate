namespace ProjectTemplate.Runtime.CrossScene.Haptic
{
	public class HapticModel : IHapticModel
	{
		private bool _isEnabled;

		private const string IS_HAPTIC_ENABLED = "IS_HAPTIC_ENABLED";
		private const string SETTINGS_PATH = "SETTINGS_PATH";
		public bool isEnabled => _isEnabled;

		public HapticModel()
		{
			_isEnabled = ES3.Load(IS_HAPTIC_ENABLED, SETTINGS_PATH, true);
		}

		public void ChangeHapticActivation()
		{
			_isEnabled = !_isEnabled;
			ES3.Save(IS_HAPTIC_ENABLED, _isEnabled, SETTINGS_PATH);
		}

	}
}