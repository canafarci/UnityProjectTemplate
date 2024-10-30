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

        private bool _isSoundEnabled;
        private bool _isMusicEnabled;
        
        public bool isSoundEnabled => _isSoundEnabled;
        public bool isMusicEnabled => _isMusicEnabled;
        
        public AudioModel(AudioDataSO audioData)
        {
            _audioData = audioData;
            _audioMixer = _audioData.audioMixer; 
            
            _isSoundEnabled = ES3.Load(nameof(_isSoundEnabled), SOUND_PATH, true);
            _isMusicEnabled = ES3.Load(nameof(_isMusicEnabled), SOUND_PATH, true);
            
            InitializeAudioActivation();
        }

        private void InitializeAudioActivation()
        {
            if(!_isMusicEnabled)
                MuteMusic();
            if(!_isSoundEnabled)
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
            if (_isSoundEnabled)
                MuteSound();
            else
                UnMuteSound();
        }

        public void ChangeMusicActivation()
        {
            if (_isMusicEnabled)
                MuteMusic();
            else
                UnMuteMusic();
        }
        
        private void MuteSound()
        {
            _audioMixer.SetFloat(SOUND_PARAM, -80);
            _isSoundEnabled = false;
            ES3.Save(nameof(_isSoundEnabled), _isSoundEnabled, SOUND_PATH);
        }

        private void UnMuteSound()
        {
            _audioMixer.SetFloat(SOUND_PARAM, 0);
            _isSoundEnabled = true;
            ES3.Save(nameof(_isSoundEnabled), _isSoundEnabled, SOUND_PATH);
        }

        private void MuteMusic()
        {
            _audioMixer.SetFloat(MUSIC_PARAM, -80);
            _isMusicEnabled = false;
            ES3.Save(nameof(_isMusicEnabled), _isMusicEnabled, SOUND_PATH);
        }

        private void UnMuteMusic()
        {
            _audioMixer.SetFloat(MUSIC_PARAM, 0);
            _isMusicEnabled = true;
            ES3.Save(nameof(_isMusicEnabled), _isMusicEnabled, SOUND_PATH);
        }
    }
}