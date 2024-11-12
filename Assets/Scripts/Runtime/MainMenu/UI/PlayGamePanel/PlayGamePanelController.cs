using System;
using ProjectTemplate.Runtime.CrossScene.Data;
using ProjectTemplate.Runtime.Infrastructure.Signals;
using ProjectTemplate.Runtime.MainMenu.Signals;
using VContainer.Unity;

namespace ProjectTemplate.Runtime.MainMenu.UI.PlayGamePanel
{
	public class PlayGamePanelController : IInitializable, IDisposable
	{
		private readonly SignalBus _signalBus;
		private readonly PlayGamePanelView _view;
		private readonly IGameplayPersistentData _gameplayPersistentData;

		public PlayGamePanelController(SignalBus signalBus,
			PlayGamePanelView view,
			IGameplayPersistentData gameplayPersistentData)
		{
			_signalBus = signalBus;
			_view = view;
			_gameplayPersistentData = gameplayPersistentData;
		}

		public void Initialize()
		{
			_view.playGameButton.onClick.AddListener(OnPlayGameButtonClickedHandler);
			SetUpAppearance();
		}

		private void SetUpAppearance()
		{
			int levelVisualDisplayNumber = _gameplayPersistentData.levelVisualDisplayNumber;
			_view.firstLevelText.SetText($"{levelVisualDisplayNumber}");
			_view.secondLevelText.SetText($"{levelVisualDisplayNumber + 1}");
			_view.thirdLevelText.SetText($"{levelVisualDisplayNumber + 2}");
		}

		private void OnPlayGameButtonClickedHandler()
		{
			_view.playGameButton.interactable = false;
			_signalBus.Fire(new TriggerExitMainMenuSignal());
		}

		public void Dispose()
		{
			_view.playGameButton.onClick.RemoveAllListeners();
		}
	}
}