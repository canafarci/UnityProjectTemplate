using TMPro;
using UnityEngine;

namespace MainMenu.CurrencyUI
{
	public class CurrencyUIView : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI CurrencyText;
		public TextMeshProUGUI currencyText => CurrencyText;
	}
}