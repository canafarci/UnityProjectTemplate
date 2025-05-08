using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectTemplate.Runtime.Gameplay.UI.ProgressionPanel
{
	public class ProgressionPanelView : MonoBehaviour
	{
		[SerializeField] private GameObject PanelRoot;
		[SerializeField] private Image Slider;
		[SerializeField] private Image Icon;
		[SerializeField] private TextMeshProUGUI TitleText;
		[SerializeField] private TextMeshProUGUI CountText;

		public GameObject panelRoot => PanelRoot;
		public Image slider => Slider;
		public Image icon => Icon;
		public TextMeshProUGUI titleText => TitleText;
		public TextMeshProUGUI countText => CountText;
	}
}