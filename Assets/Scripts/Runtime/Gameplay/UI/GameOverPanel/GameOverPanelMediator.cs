using System;
using DG.Tweening;
using ProjectTemplate.Runtime.CrossScene.Progress;
using VContainer.Unity;

namespace ProjectTemplate.Runtime.Gameplay.UI.GameOverPanel
{
	public class GameOverPanelMediator : IInitializable, IDisposable
	{
		private readonly GameOverPanelView _view;
		private readonly IProgressModel _progressModel;

		public event Action OnContinueButtonClicked;
		
		public void Initialize()
		{
			_view.continueButton.onClick.AddListener(() => OnContinueButtonClicked?.Invoke());
		}

		public GameOverPanelMediator(GameOverPanelView view,
				IProgressModel progressModel)
		{
			_view = view;
			_progressModel = progressModel;
		}

		public void ActivateGameOverPanel(bool isGameWon, int levelIndex)
		{
			_view.gameObject.SetActive(true);

			SetVisuals(isGameWon, levelIndex);
		}

		private void SetVisuals(bool isGameWon, int levelIndex)
		{
			_view.gameObject.SetActive(true);
			_view.timeline.Play();
			
			_view.gameResultImage.sprite = isGameWon
				                               ? _view.gameWonSprite
				                               : _view.gameLostSprite;
				
			SetTexts(isGameWon, levelIndex);
			ChangeObjectActivation(isGameWon);
		}
		
		private void SetProgressBarVisuals()
		{
			_view.progressBarIcon.sprite = _progressModel.progressElementIcon;
			_view.progressBarTitleText.text = _progressModel.progressElementName;
			_view.progressBarSlider.fillAmount = _progressModel.progressIndex / (float)_progressModel.progressCountToUnlock;
			
			SetCountText();
		}

		public void UpdateProgressBar(bool isGameWon, bool isUnlocked)
		{
			if (_progressModel.allElementsUnlocked)
			{
				_view.progressPanel.SetActive(false);
				return;
			}
			
			SetProgressBarVisuals();
			int progressCountToUnlock = _progressModel.progressCountToUnlock;

			if (isGameWon)
			{
				if (isUnlocked)
				{
					HandleNewElementUnlock(progressCountToUnlock);
				}
				else
				{
					HandleProgressIncrease();
				}
			}
		}



		private void SetCountText()
		{
			string countText = $"{_progressModel.progressIndex}/{_progressModel.progressCountToUnlock}";
			_view.progressionCountText.SetText(countText);
		}
		
		private void SetCountText(int maxValue)
		{
			string countText = $"{maxValue}/{maxValue}";
			_view.progressionCountText.SetText(countText);
		}

		private void HandleNewElementUnlock(int progressCountToUnlock)
		{
			DOTween.To(()=> _view.progressBarSlider.fillAmount, x=> _view.progressBarSlider.fillAmount = x, 1, 1);
			_view.progressBarTitleText.text = "Unlocked!";
			SetCountText(progressCountToUnlock);
		}
		
		private void HandleProgressIncrease()
		{
			float targetFillAmount = _progressModel.progressIndex / (float)_progressModel.progressCountToUnlock;
			DOTween.To(()=> _view.progressBarSlider.fillAmount, x=> _view.progressBarSlider.fillAmount = x, targetFillAmount, 1);
			
			SetCountText();
		}

		private void ChangeObjectActivation(bool isGameWon)
		{
			_view.gameWonSecondaryImage.SetActive(isGameWon);
			_view.gameLostSecondaryImage.SetActive(!isGameWon);
			_view.particle.SetActive(isGameWon);

		}

		private void SetTexts(bool isGameWon, int levelIndex)
		{
			string gameOverText = isGameWon ? "Completed!" : "Failed!";
			_view.gameResultText.SetText(gameOverText);
			
			_view.buttonText.text = isGameWon
				? "Continue"
				: "Try Again";
			
			string levelText = $"Level {levelIndex}";
			_view.wonLevelText.SetText(levelText);
			_view.lostLevelText.SetText(levelText);
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