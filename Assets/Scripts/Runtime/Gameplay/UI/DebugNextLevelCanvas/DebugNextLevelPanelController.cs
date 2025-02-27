using System;
using ProjectTemplate.Runtime.Gameplay.Signals;
using ProjectTemplate.Runtime.Infrastructure.Signals;
using VContainer;
using VContainer.Unity;

namespace ProjectTemplate.Runtime.Gameplay.UI.DebugNextLevelCanvas
{
	public class DebugNextLevelPanelController : IInitializable, IDisposable
	{
		[Inject] private SignalBus _signalBus;
		
		private readonly DebugNextLevelPanelView _view;

		public DebugNextLevelPanelController(DebugNextLevelPanelView view)
		{
			_view = view;
		}
		
		public void Initialize()
		{
			_view.nextLevelButton.onClick.AddListener(OnNextLevelButtonClickedHandler);
		}

		private void OnNextLevelButtonClickedHandler()
		{
			_signalBus.Fire(new TriggerLevelEndSignal(isGameWon: true));
			_view.nextLevelButton.interactable = false;
		}

		public void Dispose()
		{
			_view.nextLevelButton.onClick.RemoveAllListeners();
		}
	}
}