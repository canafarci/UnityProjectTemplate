namespace ProjectTemplate.Runtime.CrossScene.Notifications
{
	public interface INotificationModel
	{
		public bool isEnabled { get; }
		public void ChangeNotificationActivation();
	}
}