using UnityEngine;

using VContainer;
using VContainer.Unity;

using ProjectTemplate.Runtime.CrossScene;
using ProjectTemplate.Runtime.CrossScene.Audio;
using ProjectTemplate.Runtime.CrossScene.Currency;
using ProjectTemplate.Runtime.CrossScene.Data;
using ProjectTemplate.Runtime.CrossScene.Haptic;
using ProjectTemplate.Runtime.CrossScene.LoadingScreen;
using ProjectTemplate.Runtime.CrossScene.Notifications;
using ProjectTemplate.Runtime.CrossScene.Progress;
using ProjectTemplate.Runtime.CrossScene.Scenes;
using ProjectTemplate.Runtime.CrossScene.Signals;
using ProjectTemplate.Runtime.Infrastructure.ApplicationState;
using ProjectTemplate.Runtime.Infrastructure.Data;
using ProjectTemplate.Runtime.Infrastructure.MemoryPool;
using ProjectTemplate.Runtime.Infrastructure.MemoryPool.Data;
using ProjectTemplate.Runtime.Infrastructure.Signals;

namespace ProjectTemplate.Runtime.Scopes
{
	public class ApplicationScope : LifetimeScope
	{
		[SerializeField] private ApplicationSettings ApplicationSettings;
		[SerializeField] private AudioDataSO AudioDataSO;
		[SerializeField] private AudioView AudioView;
		[SerializeField] private PoolConfig PoolConfig;
		[SerializeField] private CurrencyConfig CurrencyConfig;
		[SerializeField] private ProgressData ProgressData;
		[SerializeField] private AddressableReferences AddressableReferences;

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
			builder.RegisterInstance(CurrencyConfig);
			builder.RegisterInstance(ProgressData);
			builder.RegisterInstance(AddressableReferences);
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
			builder.Register<ICurrencyModel, CurrencyModel>(Lifetime.Singleton);
			builder.Register<IProgressModel, ProgressModel>(Lifetime.Singleton);
			builder.Register<ProgressService>(Lifetime.Singleton).AsSelf();
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
			builder.DeclareSignal<CurrencyChangedSignal>();
			builder.DeclareSignal<CloseLoadingScreenSignal>();
		}
		
		private void RegisterLoadingScreen(IContainerBuilder builder)
		{
			if (!ApplicationSettings.ShowLoadingScreen) return;

			builder.RegisterComponentInNewPrefab(ApplicationSettings.LoadingScreenPrefab, Lifetime.Scoped).DontDestroyOnLoad();
			builder.RegisterEntryPoint<LoadingScreenController>();
			builder.DeclareSignal<LoadingStartedSignal>();
		}
	}
}