using UnityEngine;
using UnityEngine.UI;

namespace ProjectTemplate.Runtime.Gameplay.UI.DebugNextLevelCanvas
{
	public class DebugNextLevelPanelView : MonoBehaviour
	{
		[SerializeField] private Button NextLevelButton;

		public Button nextLevelButton => NextLevelButton;
	}
}