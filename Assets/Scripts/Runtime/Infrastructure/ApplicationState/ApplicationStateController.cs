using ProjectTemplate.Runtime.Infrastructure.ApplicationState.Signals;
using ProjectTemplate.Runtime.Infrastructure.Templates;

namespace ProjectTemplate.Runtime.Infrastructure.ApplicationState
{
	public class ApplicationStateController : SignalListener
	{
		private AppStateID _currentStateID = AppStateID.Initializing;
		
		protected override void SubscribeToEvents()
		{
			_signalBus.Subscribe<ChangeAppStateSignal>(OnAppStateChangeSignal);
		}

		private void OnAppStateChangeSignal(ChangeAppStateSignal signal)
		{
			AppStateID oldState = _currentStateID;
			_currentStateID = signal.newState;
			
			_signalBus.Fire(new AppStateChangedSignal(_currentStateID, oldState));
		}

		protected override void UnsubscribeFromEvents()
		{
			_signalBus.Unsubscribe<ChangeAppStateSignal>(OnAppStateChangeSignal);
		}
	}
}