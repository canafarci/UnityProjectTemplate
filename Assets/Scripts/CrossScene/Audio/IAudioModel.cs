using ProjectTemplate.CrossScene.Enums;
using UnityEngine;

namespace ProjectTemplate.CrossScene.Audio
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