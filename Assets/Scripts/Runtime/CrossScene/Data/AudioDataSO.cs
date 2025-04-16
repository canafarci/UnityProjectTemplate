using System.Collections.Generic;
using ProjectTemplate.Runtime.CrossScene.Enums;
using ProjectTemplate.Runtime.Infrastructure.EnumFieldAdder;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Audio;

namespace ProjectTemplate.Runtime.CrossScene.Data
{
	[CreateAssetMenu(fileName = "Audio Data", menuName = "Infrastructure/Sounds Data", order = 0)]
	public class AudioDataSO : SerializedScriptableObject
	{
		[SerializeField] private AudioMixer AudioMixer;
		[SerializeField] private Dictionary<AudioClipID, AudioClipData> AudioClips = new ();
        
		public AudioMixer audioMixer => AudioMixer;
		public Dictionary<AudioClipID, AudioClipData> audioClips => AudioClips;
        
		[Space(100f)]
		[InfoBox("You can use the input field below to add new Audio Clip IDs")]
		[ShowInInspector] private EnumFieldAdder<AudioClipID> _currencyIDAdder = new();
	}

	public struct AudioClipData
	{
		public AudioClip AudioClip;
		public float Volume;
	}
}