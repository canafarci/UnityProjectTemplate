using ProjectTemplate.Runtime.CrossScene.Enums;
using UnityEngine;

namespace ProjectTemplate.Runtime.CrossScene.Audio
{
    public interface IAudioModel
    {
        public bool isSoundEnabled { get; }
        public bool isMusicEnabled { get; }
        public AudioClip GetAudioClip(AudioClipID clipID);
        public void ChangeSoundActivation();
        public void ChangeMusicActivation();
    }
}