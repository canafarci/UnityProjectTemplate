using ProjectTemplate.Runtime.CrossScene.Currency;
using ProjectTemplate.Runtime.CrossScene.Enums;
using ProjectTemplate.Runtime.CrossScene.Signals;
using ProjectTemplate.Runtime.Infrastructure.Templates;

namespace ProjectTemplate.Runtime.MainMenu.UI.CurrencyPanel
{
	public class CurrencyPanelController : SignalListener
	{
		private readonly CurrencyPanelView _view;
		private readonly ICurrencyModel _currencyModel;
		
		public CurrencyPanelController(CurrencyPanelView view, ICurrencyModel currencyModel )
		{
			_view = view;
			_currencyModel = currencyModel;
		}
		
		public override void Initialize()
		{
			base.Initialize();
			DisplayCurrency();
		}

		protected override void SubscribeToEvents()
		{
			_signalBus.Subscribe<CurrencyChangedSignal>(OnCurrencyChangedSignalHandler);
		}

		protected override void UnsubscribeFromEvents()
		{
			_signalBus.Unsubscribe<CurrencyChangedSignal>(OnCurrencyChangedSignalHandler);
		}
		
		private void OnCurrencyChangedSignalHandler(CurrencyChangedSignal signal)
		{
			DisplayCurrency();
		}

		private void DisplayCurrency()
		{
			_view.currencyText.text = _currencyModel.GetCurrencyValue(CurrencyID.Money).ToString();
		}
	}
}