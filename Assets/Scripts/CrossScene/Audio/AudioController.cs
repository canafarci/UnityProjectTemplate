using UnityEngine;

using System;

using VContainer;
using VContainer.Unity;

using ProjectTemplate.CrossScene.Enums;
using ProjectTemplate.CrossScene.Signals;
using ProjectTemplate.Infrastructure.SignalBus;

namespace ProjectTemplate.CrossScene.Audio
{
    public class AudioController : IInitializable, IDisposable
    {
        [Inject] SignalBus _signalBus;
        
        private readonly IAudioModel _audioModel;
        private readonly AudioMediator _mediator;

        public AudioController(IAudioModel audioModel, AudioMediator mediator)
        {
            _audioModel = audioModel;
            _mediator = mediator;
        }

        public void Initialize()
        {
            _signalBus.Subscribe<ChangeAudioSettingsSignal>(AudioSettingsChangeHandler);
            _signalBus.Subscribe<PlayAudioSignal>(PlayAudioMessageHandler);
        }

        private void PlayAudioMessageHandler(PlayAudioSignal signal)
        {
            AudioClip audioClip = _audioModel.GetAudioClip(signal.audioClipID);

            switch (signal.audioSourceType)
            {
                case AudioSourceType.SoundEffect:
                    _mediator.PlaySound(audioClip, signal.volume);
                    break;
                case AudioSourceType.Music:
                    _mediator.PlayMusic(audioClip, signal.volume);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }        
        }

        private void AudioSettingsChangeHandler(ChangeAudioSettingsSignal signal)
        {
            switch (signal.audioSourceType)
            {
                case AudioSourceType.SoundEffect:
                    _audioModel.ChangeSoundActivation();
                    break;
                case AudioSourceType.Music:
                    _audioModel.ChangeMusicActivation();

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }        
        }
        
        public void Dispose()
        {
            _signalBus.Unsubscribe<ChangeAudioSettingsSignal>(AudioSettingsChangeHandler);
            _signalBus.Unsubscribe<PlayAudioSignal>(PlayAudioMessageHandler);
        }
    }
}