using ProjectTemplate.CrossScene.Enums;
using UnityEngine;

namespace ProjectTemplate.CrossScene.Messages
{
	public readonly struct PlayAudioSignal
	{
		private readonly AudioSourceType _audioSourceType;
		private readonly AudioClipID _audioClipID;
		private readonly float _volume;

		public AudioSourceType audioSourceType => _audioSourceType;
		public AudioClipID audioClipID => _audioClipID;
		public float volume => _volume;
		
		public PlayAudioSignal(AudioSourceType audioSourceType, AudioClipID audioClipID, float volume = 1f)
		{
			_audioSourceType = audioSourceType;
			_audioClipID = audioClipID;
			_volume = volume;
		}

	}
}