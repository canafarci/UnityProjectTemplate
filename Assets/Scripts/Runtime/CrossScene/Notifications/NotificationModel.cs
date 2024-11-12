namespace WarRush.CrossScene.Notifications
{
	public class NotificationModel : INotificationModel
	{
		public bool isEnabled => _isOn;
		private bool _isOn;

		private const string IS_NOTIFICATIONS_ON = "IS_NOTIFICATIONS_ON";
		private const string SETTINGS_PATH = "SETTINGS_PATH";
		
		public NotificationModel()
		{
			_isOn = ES3.Load(IS_NOTIFICATIONS_ON, SETTINGS_PATH, true);

		}
		
		public void ChangeNotificationActivation()
		{
			_isOn = !_isOn;
			ES3.Save(IS_NOTIFICATIONS_ON, _isOn, SETTINGS_PATH);
		}
	}
}