using UnityEngine;

using VContainer;
using VContainer.Unity;

using ProjectTemplate.CrossScene;
using ProjectTemplate.CrossScene.Audio;
using ProjectTemplate.CrossScene.Data;
using ProjectTemplate.CrossScene.Haptic;
using ProjectTemplate.CrossScene.Signals;
using ProjectTemplate.Gameplay.Signals;
using ProjectTemplate.Infrastructure.Pool;
using ProjectTemplate.Infrastructure.Signals;

namespace ProjectTemplate.Scopes
{
	public class ApplicationScope : LifetimeScope
	{
		[SerializeField] private AudioDataSO AudioDataSO;
		[SerializeField] private AudioView AudioView;
		[SerializeField] private PoolConfig PoolConfig;

		protected override void Configure(IContainerBuilder builder)
		{
			RegisterInstances(builder);
			RegisterEntryPoints(builder);
			RegisterServices(builder);
			
			RegisterSignalBus(builder);
			RegisterPoolManager(builder);
			RegisterSignals(builder);
		}

		private void RegisterInstances(IContainerBuilder builder)
		{
			builder.RegisterInstance(AudioDataSO);
			builder.RegisterInstance(AudioView);
		}
		
		private static void RegisterEntryPoints(IContainerBuilder builder)
		{
			builder.RegisterEntryPoint<SceneLoader>().AsSelf(); 
			builder.RegisterEntryPoint<AudioController>().AsSelf();
			builder.RegisterEntryPoint<HapticController>().AsSelf();
		}
		
		private void RegisterServices(IContainerBuilder builder)
		{
			builder.Register<IAudioModel, AudioModel>(Lifetime.Singleton);
			builder.Register<IHapticModel, HapticModel>(Lifetime.Singleton);
			builder.Register<AudioMediator>(Lifetime.Singleton).AsSelf();

			builder.Register<IGameplayPersistentData, GameplayPersistentData>(Lifetime.Singleton);
		}
		
		private void RegisterSignalBus(IContainerBuilder builder) => builder.RegisterSignalBus();
		private void RegisterPoolManager(IContainerBuilder builder) => builder.RegisterPoolManager(PoolConfig);

		private void RegisterSignals(IContainerBuilder builder)
		{
			builder.DeclareSignal<LoadSceneSignal>();
			builder.DeclareSignal<ChangeAudioSettingsSignal>();
			builder.DeclareSignal<ChangeHapticActivationSignal>();
			builder.DeclareSignal<PlayAudioSignal>();
			builder.DeclareSignal<PlayHapticSignal>();
			
			//Gameplay
			builder.DeclareSignal<GameStateChangedSignal>();
			builder.DeclareSignal<ChangeGameStateSignal>();
			builder.DeclareSignal<SetGameResultSignal>();
			builder.DeclareSignal<ExitGameplayLevelSignal>();
		}
	}
}