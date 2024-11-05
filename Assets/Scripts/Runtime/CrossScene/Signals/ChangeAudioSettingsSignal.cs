using ProjectTemplate.Runtime.CrossScene.Enums;

namespace ProjectTemplate.Runtime.CrossScene.Signals
{
	public readonly struct ChangeAudioSettingsSignal
	{
		private readonly AudioSourceType _audioSourceType;

		public ChangeAudioSettingsSignal(AudioSourceType audioType)
		{
			_audioSourceType = audioType;
		}

		public AudioSourceType audioSourceType => _audioSourceType;
	}
}