using Lofelt.NiceVibrations;
using ProjectTemplate.Runtime.CrossScene.Signals;
using ProjectTemplate.Runtime.Infrastructure.Templates;
using VContainer;

namespace ProjectTemplate.Runtime.CrossScene.Haptic
{
	public class HapticController : SignalListener
	{
		[Inject] private IHapticModel _hapticModel;

		protected override void SubscribeToEvents()
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
		
		protected override void UnsubscribeFromEvents()
		{
			_signalBus.Unsubscribe<PlayHapticSignal>(OnPlayHapticMessage);
			_signalBus.Unsubscribe<ChangeHapticActivationSignal>(OnChangeHapticActivationSignal);
		}
	}
}