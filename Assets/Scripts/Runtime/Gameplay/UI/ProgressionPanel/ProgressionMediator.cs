using DG.Tweening;
using ProjectTemplate.Runtime.CrossScene.Progress;
using VContainer.Unity;

namespace ProjectTemplate.Runtime.Gameplay.UI.ProgressionPanel
{
	public class ProgressionMediator : IInitializable
	{
		private readonly ProgressionPanelView _view;
		private readonly IProgressModel _model;

		public ProgressionMediator(ProgressionPanelView view, IProgressModel model)
		{
			_view = view;
			_model = model;
		}

		public void Initialize()
		{
			_view.panelRoot.SetActive(false);
		}
		
		public void ShowProgress(bool unlocked)
		{
			// hide if no steps at all
			if (_model.progressCountToUnlock <= 0)
			{
				_view.panelRoot.SetActive(false);
				return;
			}

			// hide any post-unlock gameovers
			if (_model.allElementsUnlocked && !unlocked)
			{
				_view.panelRoot.SetActive(false);
				return;
			}

			// otherwise reveal
			_view.panelRoot.SetActive(true);

			// choose the correct icon & threshold
			if (unlocked)
			{
				_view.icon.sprite = _model.lastUnlockedElementIcon;
				_view.titleText.text = "Unlocked!";
				_view.countText.text = $"{_model.lastUnlockedThreshold}/{_model.lastUnlockedThreshold}";
			}
			else
			{
				_view.icon.sprite = _model.progressElementIcon;
				_view.titleText.text = _model.progressElementName;
				_view.countText.text = $"{_model.progressIndex}/{_model.progressCountToUnlock}";
			}

			// animate to full on unlock, or to the correct fill on progress increase
			float targetFill = unlocked
				? 1f
				: (_model.progressIndex / (float)_model.progressCountToUnlock);

			DOTween.To(() => _view.slider.fillAmount,
			           x => _view.slider.fillAmount = x,
			           targetFill, 1f);
		}
	}
}