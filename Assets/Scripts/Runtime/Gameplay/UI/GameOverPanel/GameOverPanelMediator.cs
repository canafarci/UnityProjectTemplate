using System;
using VContainer.Unity;

namespace ProjectTemplate.Runtime.Gameplay.UI.GameOverPanel
{
	public class GameOverPanelMediator : IInitializable, IDisposable
	{
		private readonly GameOverPanelView _view;

		public event Action OnContinueButtonClicked;

		public GameOverPanelMediator(GameOverPanelView view)
		{
			_view = view;
		}

		public void Initialize()
		{
			_view.continueButton.onClick.AddListener(() => OnContinueButtonClicked?.Invoke());
		}

		public void ActivateGameOverPanel(bool isGameWon, int levelIndex)
		{
			_view.gameObject.SetActive(true);
			_view.timeline.Play();

			_view.gameResultImage.sprite = isGameWon
				? _view.gameWonSprite
				: _view.gameLostSprite;

			_view.gameResultText.SetText(isGameWon ? "Completed!" : "Failed!");
			_view.buttonText.text = isGameWon ? "Continue" : "Try Again";

			string levelText = $"Level {levelIndex}";
			_view.wonLevelText.SetText(levelText);
			_view.lostLevelText.SetText(levelText);

			_view.gameWonSecondaryImage.SetActive(isGameWon);
			_view.gameLostSecondaryImage.SetActive(!isGameWon);
			_view.particle.SetActive(isGameWon);
		}

		public void DisableContinueButton()
		{
			_view.continueButton.interactable = false;
		}

		public void Dispose()
		{
			_view.continueButton.onClick.RemoveAllListeners();
		}
	}
}