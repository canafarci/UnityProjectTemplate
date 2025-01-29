using TMPro;
using UnityEngine;

namespace ProjectTemplate.Runtime.Gameplay.UI.GameplayCurrencyPanel
{
	public class GameplayCurrencyPanelView : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI CurrencyText;

		public TextMeshProUGUI currencyText => CurrencyText;
	}
}