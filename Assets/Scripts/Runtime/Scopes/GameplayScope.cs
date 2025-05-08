using ProjectTemplate.Runtime.CrossScene.Progress;
using ProjectTemplate.Runtime.Gameplay.GameplayLifecycle;
using ProjectTemplate.Runtime.Gameplay.GameplayLifecycle.GameStates;
using ProjectTemplate.Runtime.Gameplay.Signals;
using ProjectTemplate.Runtime.Gameplay.UI.DebugNextLevelCanvas;
using ProjectTemplate.Runtime.Gameplay.UI.GameOverPanel;
using ProjectTemplate.Runtime.Gameplay.UI.GameplayCurrencyPanel;
using ProjectTemplate.Runtime.Gameplay.UI.GameplayLevelDisplayPanel;
using ProjectTemplate.Runtime.Gameplay.UI.GameSceneSettings;
using ProjectTemplate.Runtime.Gameplay.UI.ProgressionPanel;
using ProjectTemplate.Runtime.Infrastructure.Initialization;
using ProjectTemplate.Runtime.Infrastructure.Signals;
using VContainer;
using VContainer.Unity;

namespace SRC.Runtime.Scopes
{
    public class GameplayScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            RegisterGameplayLifecycleManagers(builder);
            RegisterUIPanels(builder);
            RegisterSignals(builder);
        }

        private void RegisterUIPanels(IContainerBuilder builder)
        {
            RegisterInGameSettings(builder);
            RegisterGameOverPanel(builder);
            RegisterCurrencyPanel(builder);
            RegisterLevelDisplayPanel(builder);
            RegisterDebugNextLevelPanel(builder);
            RegisterProgressionPanel(builder);
        }

        private void RegisterGameplayLifecycleManagers(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<GameplayEntryPoint>();
            builder.RegisterEntryPoint<GameplayExitPoint>();
            builder.RegisterEntryPoint<GameStateController>();
            builder.RegisterEntryPoint<SceneInitializer>().AsSelf();
            builder.Register<IGameStateModel, GameStateModel>(Lifetime.Singleton);
        }

        private void RegisterInGameSettings(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<GameSceneSettingsView>().AsSelf();
            builder.RegisterEntryPoint<GameSceneSettingsMediator>().AsSelf();
            builder.RegisterEntryPoint<GameSceneSettingsController>().AsSelf();
        }

        private void RegisterGameOverPanel(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<GameOverPanelView>().AsSelf();
            builder.RegisterEntryPoint<GameOverPanelMediator>().AsSelf();
            builder.RegisterEntryPoint<GameOverPanelController>().AsSelf();
        }

        private void RegisterCurrencyPanel(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<GameplayCurrencyPanelView>().AsSelf();
            builder.RegisterEntryPoint<GameplayCurrencyPanelController>();
        }

        private void RegisterLevelDisplayPanel(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<GameplayLevelDisplayView>().AsSelf();
            builder.RegisterEntryPoint<GameplayLevelDisplayController>();        
        }

        private void RegisterDebugNextLevelPanel(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<DebugNextLevelPanelView>().AsSelf();
            builder.RegisterEntryPoint<DebugNextLevelPanelController>();        
        }

        private void RegisterProgressionPanel(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<ProgressionPanelView>().AsSelf();
            builder.RegisterEntryPoint<ProgressionMediator>().AsSelf();
            builder.RegisterEntryPoint<ProgressionController>().AsSelf();
            
            builder.Register<IProgressModel, ProgressionModel>(Lifetime.Singleton);
            builder.Register<ProgressionService>(Lifetime.Singleton);
        }

        private void RegisterSignals(IContainerBuilder builder)
        {
            builder.DeclareSignal<GameStateChangedSignal>();
            builder.DeclareSignal<ChangeGameStateSignal>();
            builder.DeclareSignal<TriggerLevelEndSignal>();
            builder.DeclareSignal<TriggerExitGameplayLevelSignal>();
        }
    }
}
