using System.Collections.Generic;
using ProjectTemplate.CrossScene.Enums;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Audio;

namespace ProjectTemplate.CrossScene.Data
{
    [CreateAssetMenu(fileName = "Audio Data", menuName = "Infrastructure/Sounds Data", order = 0)]
    public class AudioDataSO : SerializedScriptableObject
    {
        [SerializeField] private AudioMixer _audioMixer;
        [SerializeField] private Dictionary<AudioClipID, AudioClip> _audioClips = new ();
        
        public AudioMixer audioMixer => _audioMixer;
        public Dictionary<AudioClipID, AudioClip> audioClips => _audioClips;
    }
}