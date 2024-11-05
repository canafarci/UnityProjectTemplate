using UnityEngine;

namespace ProjectTemplate.Runtime.CrossScene.Audio
{
    public class AudioMediator
    {
        private readonly AudioView _view;

        public AudioMediator(AudioView view)
        {
            _view = view;
        }
        
        public void PlaySound(AudioClip audioClip, float volume)
        {
            _view.soundEffectAudioSource.PlayOneShot(audioClip, volume);
        }
        
        public void PlayMusic(AudioClip audioClip, float volume)
        {
            _view.musicAudioSource.clip = audioClip;
            _view.musicAudioSource.loop = true;
            _view.musicAudioSource.volume = volume;
            
            _view.musicAudioSource.Play();
        }
    }
}