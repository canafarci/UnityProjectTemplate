namespace ProjectTemplate.CrossScene.Haptic
{
	public interface IHapticModel
	{
		public bool isOn { get; }
		public void ChangeHapticActivation();
	}
}