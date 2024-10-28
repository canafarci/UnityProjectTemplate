using UnityEngine;
using UnityEngine.Audio;

using ProjectTemplate.CrossScene.Enums;
using ProjectTemplate.Data.Persistent;

namespace ProjectTemplate.CrossScene.Audio
{
    public class AudioModel :  IAudioModel
    {
        private readonly AudioDataSO _audioData;
        private readonly AudioMixer _audioMixer;
        
        private const string SOUND_PARAM = "SOUND_PARAM";
        private const string MUSIC_PARAM = "MUSIC_PARAM";
        private const string SOUND_PATH = "SOUND_PATH";
        
        private bool _isSoundMuted = ES3.Load(nameof(_isSoundMuted), SOUND_PATH, false);
        private bool _isMusicMuted = ES3.Load(nameof(_isMusicMuted), SOUND_PATH, false);
        
        public AudioModel(AudioDataSO audioData)
        {
            _audioData = audioData;
            _audioMixer = _audioData.audioMixer; 
            
            InitializeAudioActivation();
        }


        public bool isSoundMuted => _isSoundMuted;
        
        public bool isMusicMuted => _isMusicMuted;

        private void InitializeAudioActivation()
        {
            if(_isMusicMuted)
                MuteMusic();
            if(_isSoundMuted)
                MuteSound();
        }
        
        public AudioClip GetAudioClip(AudioClipID clipID)
        {
            if (_audioData.audioClips.TryGetValue(clipID, out AudioClip clip))
            {
                return clip;
            }
            else
            {
                throw new System.ArgumentException($"Audio clip with ID {clipID} not found");
            }
        }

        public void ChangeSoundActivation()
        {
            if (_isSoundMuted)
                UnMuteSound();
            else
                MuteSound();
        }

        public void ChangeMusicActivation()
        {
            if (_isMusicMuted)
                UnMuteMusic();
            else
                MuteMusic();
        }
        
        private void MuteSound()
        {
            _audioMixer.SetFloat(SOUND_PARAM, -80);
            _isSoundMuted = true;
            ES3.Save(nameof(_isSoundMuted), _isSoundMuted, SOUND_PATH);
        }

        private void UnMuteSound()
        {
            _audioMixer.SetFloat(SOUND_PARAM, 0);
            _isSoundMuted = false;
            ES3.Save(nameof(_isSoundMuted), _isSoundMuted, SOUND_PATH);
        }

        private void MuteMusic()
        {
            _audioMixer.SetFloat(MUSIC_PARAM, -80);
            _isMusicMuted = true;
            ES3.Save(nameof(_isMusicMuted), _isMusicMuted, SOUND_PATH);
        }

        private void UnMuteMusic()
        {
            _audioMixer.SetFloat(MUSIC_PARAM, 0);
            _isMusicMuted = false;
            ES3.Save(nameof(_isMusicMuted), _isMusicMuted, SOUND_PATH);
        }
    }
}