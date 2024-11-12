using System;
using ProjectTemplate.Runtime.Infrastructure.Templates;
using VContainer;
using VContainer.Unity;

namespace MainMenu.CurrencyUI
{
	public class CurrencyUIController //: SignalListener
	{
		// protected override void SubscribeToEvents()
		// {
		// 	throw new NotImplementedException();
		// }
		//
		// private readonly CurrencyUIView _view;
		// private readonly ICurrencyModel _currencyModel;
		//
		// public CurrencyUIController(CurrencyUIView view, ICurrencyModel currencyModel )
		// {
		// 	_view = view;
		// 	_currencyModel = currencyModel;
		// }
		// public void Initialize()
		// {
		// 	_currencyChangedSubscriber.Subscribe(CurrencyChangedHandler);
		// 	_view.currencyText.text = _currencyModel.currency.ToString();
		// }
		//
		//
		// private void CurrencyChangedHandler(CurrencyChangedMessage message)
		// {
		// 	_view.currencyText.text = _currencyModel.currency.ToString();
		// }
		//
		// protected override void UnsubscribeFromEvents()
		// {
		// 	throw new NotImplementedException();
		// }
	}
}