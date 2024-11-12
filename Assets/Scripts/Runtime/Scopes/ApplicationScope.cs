using UnityEngine;

using VContainer;
using VContainer.Unity;

using ProjectTemplate.Runtime.CrossScene;
using ProjectTemplate.Runtime.CrossScene.Audio;
using ProjectTemplate.Runtime.CrossScene.Data;
using ProjectTemplate.Runtime.CrossScene.Haptic;
using ProjectTemplate.Runtime.CrossScene.LoadingScreen;
using ProjectTemplate.Runtime.CrossScene.LoadingScreen.Signals;
using ProjectTemplate.Runtime.CrossScene.Notifications;
using ProjectTemplate.Runtime.CrossScene.Signals;
using ProjectTemplate.Runtime.Gameplay.Signals;
using ProjectTemplate.Runtime.Infrastructure.ApplicationState;
using ProjectTemplate.Runtime.Infrastructure.Data;
using ProjectTemplate.Runtime.Infrastructure.MemoryPool;
using ProjectTemplate.Runtime.Infrastructure.Signals;
using ProjectTemplate.Runtime.MainMenu.Signals;

namespace ProjectTemplate.Runtime.Scopes
{
	public class ApplicationScope : LifetimeScope
	{
		[SerializeField] private ApplicationSettings ApplicationSettings;
		[SerializeField] private AudioDataSO AudioDataSO;
		[SerializeField] private AudioView AudioView;
		[SerializeField] private PoolConfig PoolConfig;

		protected override void Configure(IContainerBuilder builder)
		{
			RegisterInstances(builder);
			RegisterEntryPoints(builder);
			RegisterServices(builder);
			RegisterSignals(builder);
			RegisterLoadingScreen(builder);
		}
		
		private void RegisterInstances(IContainerBuilder builder)
		{
			builder.RegisterInstance(AudioDataSO);
			builder.RegisterInstance(AudioView);
			builder.RegisterInstance(ApplicationSettings);
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
			builder.Register<INotificationModel, NotificationModel>(Lifetime.Singleton);
			builder.Register<AudioMediator>(Lifetime.Singleton).AsSelf();

			builder.Register<IGameplayPersistentData, GameplayPersistentData>(Lifetime.Singleton);
			
			builder.RegisterSignalBus();
			builder.RegisterPoolManager(PoolConfig);
			builder.RegisterAppController();
		}

		private void RegisterSignals(IContainerBuilder builder)
		{
			//cross scene
			builder.DeclareSignal<LoadSceneSignal>();
			builder.DeclareSignal<ChangeAudioSettingsSignal>();
			builder.DeclareSignal<ChangeHapticActivationSignal>();
			builder.DeclareSignal<PlayAudioSignal>();
			builder.DeclareSignal<PlayHapticSignal>();
			
			//Main Menu
			builder.DeclareSignal<TriggerExitMainMenuSignal>();
			
			//Gameplay
			builder.DeclareSignal<GameStateChangedSignal>();
			builder.DeclareSignal<ChangeGameStateSignal>();
			builder.DeclareSignal<TriggerLevelEndSignal>();
			builder.DeclareSignal<ExitGameplayLevelSignal>();
		}
		
		private void RegisterLoadingScreen(IContainerBuilder builder)
		{
			if (!ApplicationSettings.ShowLoadingScreen) return;

			builder.RegisterComponentInNewPrefab(ApplicationSettings.LoadingScreenPrefab, Lifetime.Scoped).DontDestroyOnLoad();
			builder.RegisterEntryPoint<LoadingScreenController>();
			builder.DeclareSignal<LoadingStartedSignal>();
			builder.DeclareSignal<LoadingFinishedSignal>();
		}
	}
}