using ProjectTemplate.CrossScene.Enums;
using UnityEngine;

namespace ProjectTemplate.CrossScene.Audio
{
    public interface IAudioModel
    {
        public bool isSoundMuted { get; }
        public bool isMusicMuted { get; }
        public AudioClip GetAudioClip(AudioClipID clipID);
        public void ChangeSoundActivation();
        public void ChangeMusicActivation();
    }
}