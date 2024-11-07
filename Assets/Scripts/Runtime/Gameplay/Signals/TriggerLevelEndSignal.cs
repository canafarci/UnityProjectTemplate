namespace ProjectTemplate.Runtime.Gameplay.Signals
{
	public readonly struct TriggerLevelEndSignal
	{
		private readonly bool _isGameWon;
		public bool isGameWon => _isGameWon;

		public TriggerLevelEndSignal(bool isGameWon)
		{
			_isGameWon = isGameWon;
		}
	}
}