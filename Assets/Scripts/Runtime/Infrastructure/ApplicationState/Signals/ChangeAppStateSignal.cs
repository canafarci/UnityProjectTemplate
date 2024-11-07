namespace ProjectTemplate.Runtime.Infrastructure.ApplicationState.Signals
{
	public readonly struct ChangeAppStateSignal
	{
		private readonly AppStateID _newState;

		public AppStateID newState => _newState;
		
		public ChangeAppStateSignal(AppStateID newState)
		{
			_newState = newState;
		}
	}
}