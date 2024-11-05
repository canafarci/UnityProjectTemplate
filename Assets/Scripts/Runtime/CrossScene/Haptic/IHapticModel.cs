namespace ProjectTemplate.Runtime.CrossScene.Haptic
{
	public interface IHapticModel
	{
		public bool isEnabled { get; }
		public void ChangeHapticActivation();
	}
}