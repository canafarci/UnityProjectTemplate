using UnityEngine;
using UnityEngine.Audio;

using System.Collections.Generic;
using Sirenix.OdinInspector;

using ProjectTemplate.CrossScene.Enums;

namespace ProjectTemplate.Data.Persistent
{
    [CreateAssetMenu(fileName = "Sounds Data", menuName = "ProjectTemplate/Sounds Data", order = 0)]
    public class AudioDataSO : SerializedScriptableObject
    {
        [SerializeField] private AudioMixer _audioMixer;
        [SerializeField] private Dictionary<AudioClipID, AudioClip> _audioClips = new ();
        
        public AudioMixer audioMixer => _audioMixer;
        public Dictionary<AudioClipID, AudioClip> audioClips => _audioClips;
    }
}