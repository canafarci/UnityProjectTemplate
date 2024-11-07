namespace ProjectTemplate.Runtime.Infrastructure.ApplicationState.Signals
{
	public readonly struct AppStateChangedSignal
	{
		private readonly AppStateID _newState;
		private readonly AppStateID _oldState;

		public AppStateID newState => _newState;
		public AppStateID oldState => _oldState;

		public AppStateChangedSignal(AppStateID newState, AppStateID oldState)
		{
			_newState = newState;
			_oldState = oldState;
		}

	}
}