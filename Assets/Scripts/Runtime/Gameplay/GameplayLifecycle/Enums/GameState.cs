namespace ProjectTemplate.Runtime.Gameplay.GameplayLifecycle.Enums
{
	public enum GameState : byte
	{
		Initializing = 0x00,
		Playing = 0x01,
		Paused = 0x02,
		GameOver = 0x03,
		Reloading = 0x04,
	}
}