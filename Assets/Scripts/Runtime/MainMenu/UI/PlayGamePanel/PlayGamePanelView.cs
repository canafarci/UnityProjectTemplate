using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectTemplate.Runtime.MainMenu.UI.PlayGamePanel
{
	public class PlayGamePanelView : MonoBehaviour
	{
		[SerializeField] private Button PlayGameButton;
		[SerializeField] private TextMeshProUGUI FirstLevelText;
		[SerializeField] private TextMeshProUGUI SecondLevelText;
		[SerializeField] private TextMeshProUGUI ThirdLevelText;


		public Button playGameButton => PlayGameButton;
		public TextMeshProUGUI firstLevelText => FirstLevelText;
		public TextMeshProUGUI secondLevelText => SecondLevelText;
		public TextMeshProUGUI thirdLevelText => ThirdLevelText;
	}
}