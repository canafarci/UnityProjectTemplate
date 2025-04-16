using ProjectTemplate.Runtime.CrossScene.Data;

namespace ProjectTemplate.Runtime.CrossScene.Audio
{
	public class AudioMediator
	{
		private readonly AudioView _view;

		public AudioMediator(AudioView view)
		{
			_view = view;
		}
        
		public void PlaySound(AudioClipData data)
		{
			_view.soundEffectAudioSource.PlayOneShot(data.AudioClip, data.Volume);
		}
        
		public void PlayMusic(AudioClipData data)
		{
			_view.musicAudioSource.clip = data.AudioClip;
			_view.musicAudioSource.loop = true;
			_view.musicAudioSource.volume = data.Volume;
            
			_view.musicAudioSource.Play();
		}
	}
}