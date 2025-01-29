using ProjectTemplate.Runtime.CrossScene.Currency;
using ProjectTemplate.Runtime.CrossScene.Enums;
using VContainer.Unity;

namespace ProjectTemplate.Runtime.Gameplay.UI.GameplayCurrencyPanel
{
	public class GameplayCurrencyPanelController : IInitializable
	{
		private readonly GameplayCurrencyPanelView _view;
		private readonly ICurrencyModel _model;

		public GameplayCurrencyPanelController(GameplayCurrencyPanelView view, ICurrencyModel model)
		{
			_view = view;
			_model = model;
		}

		public void Initialize()
		{
			string currencyValue = _model.GetCurrencyValue(CurrencyID.Money).ToString();
			_view.currencyText.SetText(currencyValue);
		}
	}
}