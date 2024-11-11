using System;
using ProjectTemplate.Runtime.Infrastructure.Signals;
using ProjectTemplate.Runtime.MainMenu.Signals;
using VContainer.Unity;

namespace ProjectTemplate.Runtime.MainMenu.PlayGameCanvas
{
	public class PlayGameCanvasController : IInitializable, IDisposable
	{
		private readonly SignalBus _signalBus;
		private readonly PlayGameCanvasView _view;

		public PlayGameCanvasController(SignalBus signalBus, PlayGameCanvasView view)
		{
			_signalBus = signalBus;
			_view = view;
		}

		public void Initialize()
		{
			_view.playGameButton.onClick.AddListener(OnPlayGameButtonClickedHandler);
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