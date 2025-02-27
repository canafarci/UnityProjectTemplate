using TMPro;
using UnityEngine;

namespace ProjectTemplate.Runtime.Gameplay.UI.GameplayLevelDisplayPanel
{
	public class GameplayLevelDisplayView : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI LevelText;

		public TextMeshProUGUI levelText => LevelText;
	}
}