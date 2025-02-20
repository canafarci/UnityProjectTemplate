namespace ProjectTemplate.Runtime.Gameplay.Signals
{
	public struct TriggerExitGameplayLevelSignal
	{
		public bool reload { get; }
		public TriggerExitGameplayLevelSignal(bool reload = false)
		{
			this.reload = reload;
		}
	}
}