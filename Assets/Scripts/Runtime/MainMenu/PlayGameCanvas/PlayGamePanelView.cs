using UnityEngine;
using UnityEngine.UI;

namespace ProjectTemplate.Runtime.MainMenu.PlayGameCanvas
{
	public class PlayGamePanelView : MonoBehaviour
	{
		[SerializeField] Button PlayGameButton;

		public Button playGameButton => PlayGameButton;
	}
}