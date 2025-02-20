using ProjectTemplate.Runtime.CrossScene.Data;
using ProjectTemplate.Runtime.CrossScene.Scenes.Enums;
using ProjectTemplate.Runtime.CrossScene.Signals;
using ProjectTemplate.Runtime.Infrastructure.Templates;
using ProjectTemplate.Runtime.MainMenu.Signals;

namespace ProjectTemplate.Runtime.MainMenu.MainMenuLifecycle
{
	public class MainMenuExitPoint : SignalListener
	{
		private readonly IGameplayPersistentData _gameplayPersistentData;

		public MainMenuExitPoint(IGameplayPersistentData gameplayPersistentData)
		{
			_gameplayPersistentData = gameplayPersistentData;
		}
		protected override void SubscribeToEvents()
		{
			_signalBus.Subscribe<TriggerExitMainMenuSignal>(OnTriggerExitMainMenuSignalHandler);
		}

		private void OnTriggerExitMainMenuSignalHandler(TriggerExitMainMenuSignal signal)
		{
			_signalBus.Fire(new LoadSceneSignal(SceneID.Gameplay));
		}

		protected override void UnsubscribeFromEvents()
		{
			_signalBus.Unsubscribe<TriggerExitMainMenuSignal>(OnTriggerExitMainMenuSignalHandler);
		}
	}
}