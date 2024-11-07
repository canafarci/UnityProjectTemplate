namespace ProjectTemplate.Runtime.Infrastructure.ApplicationState
{
	public enum AppStateID : byte
	{
		Initializing = 0x00,
		Loading = 0x01,
		MainMenu = 0x02,
		Gameplay = 0x03,
	}
}