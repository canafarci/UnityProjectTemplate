using System;
using VContainer.Unity;

namespace ProjectTemplate.Gameplay.UI.GameOverPanel
{
	public class GameOverPanelMediator : IInitializable, IDisposable
	{
		private readonly GameOverPanelView _view;
		
		public event Action OnContinueButtonClicked;
		
		public void Initialize()
		{
			_view.continueButton.onClick.AddListener(() => OnContinueButtonClicked?.Invoke());
		}

		public GameOverPanelMediator(GameOverPanelView view)
		{
			_view = view;
		}

		public void ActivateGameOverPanel(bool isGameWon)
		{
			_view.gameObject.SetActive(true);

			SetVisuals(isGameWon);
		}

		private void SetVisuals(bool isGameWon)
		{
			string gameOverText = isGameWon ? "Completed!" : "Failed!";
			_view.timeline.playableAsset = isGameWon ? _view.gameWonPlayableAsset : _view.gameLostPlayableAsset;
			_view.timeline.Play();
			
			_view.gameResultText.SetText(gameOverText);
			_view.gameResultImage.sprite = isGameWon
				                               ? _view.gameWonSprite
				                               : _view.gameLostSprite;
			
			_view.gameObject.SetActive(true);
			_view.timeline.Play();
		}
		
		public void Dispose()
		{
			_view.continueButton.onClick.RemoveAllListeners();
		}

		public void DisableContinueButton()
		{
			_view.continueButton.interactable = false;
		}
	}
}