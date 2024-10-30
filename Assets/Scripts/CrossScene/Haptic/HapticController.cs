using System;

using Lofelt.NiceVibrations;

using VContainer;
using VContainer.Unity;

using ProjectTemplate.Infrastructure.SignalBus;
using ProjectTemplate.CrossScene.Signals;

namespace ProjectTemplate.CrossScene.Haptic
{
	public class HapticController : IInitializable, IDisposable
	{
		[Inject] private SignalBus _signalBus;
		[Inject] private IHapticModel _hapticModel;
		
		public void Initialize()
		{
			_signalBus.Subscribe<PlayHapticSignal>(OnPlayHapticMessage);
			_signalBus.Subscribe<ChangeHapticActivationSignal>(OnChangeHapticActivationSignal);
		}
		
		private void OnPlayHapticMessage(PlayHapticSignal signal)
		{
			if (!_hapticModel.isEnabled) return;
            
			HapticPatterns.PlayPreset(signal.hapticType);
		}
		
		private void OnChangeHapticActivationSignal(ChangeHapticActivationSignal obj)
		{
			_hapticModel.ChangeHapticActivation();
		}

		public void Dispose()
		{
			_signalBus.Unsubscribe<PlayHapticSignal>(OnPlayHapticMessage);
			_signalBus.Unsubscribe<ChangeHapticActivationSignal>(OnChangeHapticActivationSignal);
		}
	}
}