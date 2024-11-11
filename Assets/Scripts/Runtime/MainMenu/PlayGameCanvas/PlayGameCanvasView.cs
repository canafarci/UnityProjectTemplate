using UnityEngine;
using UnityEngine.UI;

namespace ProjectTemplate.Runtime.MainMenu.PlayGameCanvas
{
	public class PlayGameCanvasView : MonoBehaviour
	{
		[SerializeField] Button PlayGameButton;

		public Button playGameButton => PlayGameButton;
	}
}