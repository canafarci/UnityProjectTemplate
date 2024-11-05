namespace ProjectTemplate.Runtime.Gameplay.Signals
{
	public readonly struct SetGameResultSignal
	{
		private readonly bool _isGameWon;
		public bool isGameWon => _isGameWon;

		public SetGameResultSignal(bool isGameWon)
		{
			_isGameWon = isGameWon;
		}
	}
}