using Lofelt.NiceVibrations;

using VContainer;

using ProjectTemplate.CrossScene.Signals;
using ProjectTemplate.Infrastructure.Templates;

namespace ProjectTemplate.CrossScene.Haptic
{
	public class HapticController : SignalListener
	{
		[Inject] private IHapticModel _hapticModel;

		protected override void SubscribeToSignals()
		{
			_signalBus.Subscribe<PlayHapticSignal>(OnPlayHapticMessage);
			_signalBus.Subscribe<ChangeHapticActivationSignal>(OnChangeHapticActivationSignal);		
		}
		
		private void OnPlayHapticMessage(PlayHapticSignal signal)
		{
			if (!_hapticModel.isEnabled) return;
            
			HapticPatterns.PlayPreset(signal.hapticType);
		}
		
		private void OnChangeHapticActivationSignal(ChangeHapticActivationSignal signal)
		{
			_hapticModel.ChangeHapticActivation();
		}
		
		protected override void UnsubscribeToSignals()
		{
			_signalBus.Unsubscribe<PlayHapticSignal>(OnPlayHapticMessage);
			_signalBus.Unsubscribe<ChangeHapticActivationSignal>(OnChangeHapticActivationSignal);
		}
	}
}