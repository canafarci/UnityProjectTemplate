using UnityEngine;

using VContainer;
using VContainer.Unity;

using ProjectTemplate.CrossScene;
using ProjectTemplate.CrossScene.Audio;
using ProjectTemplate.CrossScene.Messages;
using ProjectTemplate.Data.Persistent;
using ProjectTemplate.Infrastructure.SignalBus;

namespace ProjectTemplate.Scopes
{
	public class ApplicationScope : LifetimeScope
	{
		[SerializeField] AudioDataSO _audioDataSO;
		[SerializeField] private AudioView _audioView;

		protected override void Configure(IContainerBuilder builder)
		{
			RegisterInstances(builder);
			RegisterEntryPoints(builder);
			RegisterServices(builder);
			RegisterSignalBus(builder);
			RegisterSignals(builder);
		}

		private void RegisterInstances(IContainerBuilder builder)
		{
			builder.RegisterInstance(_audioDataSO);
			builder.RegisterInstance(_audioView);
		}
		
		private static void RegisterEntryPoints(IContainerBuilder builder)
		{
			builder.RegisterEntryPoint<SceneLoader>().AsSelf(); 
			builder.RegisterEntryPoint<AudioController>().AsSelf();
		}
		
		private void RegisterServices(IContainerBuilder builder)
		{
			builder.Register<IAudioModel, AudioModel>(Lifetime.Singleton).AsSelf();
			builder.Register<AudioMediator>(Lifetime.Singleton).AsSelf();
		}
		
		private void RegisterSignalBus(IContainerBuilder builder) => builder.RegisterSignalBus();

		private void RegisterSignals(IContainerBuilder builder)
		{
			builder.DeclareSignal<LoadSceneSignal>();
			builder.DeclareSignal<ChangeAudioSettingsSignal>();
			builder.DeclareSignal<PlayAudioSignal>();
		}
	}
}