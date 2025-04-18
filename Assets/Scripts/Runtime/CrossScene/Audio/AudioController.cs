using System;
using ProjectTemplate.Runtime.CrossScene.Data;
using ProjectTemplate.Runtime.CrossScene.Enums;
using ProjectTemplate.Runtime.CrossScene.Signals;
using ProjectTemplate.Runtime.Infrastructure.Templates;

namespace ProjectTemplate.Runtime.CrossScene.Audio
{
    public class AudioController : SignalListener
    {
        private readonly IAudioModel _audioModel;
        private readonly AudioMediator _mediator;

        public AudioController(IAudioModel audioModel, AudioMediator mediator)
        {
            _audioModel = audioModel;
            _mediator = mediator;
        }

        protected override void SubscribeToEvents()
        {
            _signalBus.Subscribe<ChangeAudioSettingsSignal>(AudioSettingsChangeHandler);
            _signalBus.Subscribe<PlayAudioSignal>(PlayAudioMessageHandler);
        }

        private void PlayAudioMessageHandler(PlayAudioSignal signal)
        {
            AudioClipData data = _audioModel.GetAudioClip(signal.audioClipID);

            switch (signal.audioSourceType)
            {
                case AudioSourceType.SoundEffect:
                    _mediator.PlaySound(data);
                    break;
                case AudioSourceType.Music:
                    _mediator.PlayMusic(data);
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
        
        protected override void UnsubscribeFromEvents()
        {
            _signalBus.Unsubscribe<ChangeAudioSettingsSignal>(AudioSettingsChangeHandler);
            _signalBus.Unsubscribe<PlayAudioSignal>(PlayAudioMessageHandler);
        }
    }
}