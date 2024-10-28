namespace ProjectTemplate.CrossScene.Haptic
{
	public class HapticModel : IHapticModel
	{
		private bool _isOn = ES3.Load(IS_HAPTIC_ON, SETTINGS_PATH, true);

		private const string IS_HAPTIC_ON = "IS_HAPTIC_ON";
		private const string SETTINGS_PATH = "SETTINGS_PATH";
		public bool isOn => _isOn;

		public void ChangeHapticActivation()
		{
			_isOn = !_isOn;
			ES3.Save(IS_HAPTIC_ON, _isOn, SETTINGS_PATH);
		}

	}
}