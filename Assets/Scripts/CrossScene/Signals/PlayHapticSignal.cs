using Lofelt.NiceVibrations;

namespace ProjectTemplate.CrossScene.Signals
{
	public class PlayHapticSignal
	{
		private readonly HapticPatterns.PresetType _hapticType;

		public PlayHapticSignal(HapticPatterns.PresetType hapticType)
		{
			_hapticType = hapticType;
		}

		public HapticPatterns.PresetType hapticType => _hapticType;
	}
}