using TMPro;
using UnityEngine;

namespace ProjectTemplate.Runtime.MainMenu.UI.CurrencyPanel
{
	public class CurrencyPanelView : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI CurrencyText;
		public TextMeshProUGUI currencyText => CurrencyText;
	}
}