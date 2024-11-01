using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

using TMPro;

namespace ProjectTemplate.Gameplay.UI.GameOverPanel
{
	public class GameOverPanelView : MonoBehaviour
	{
		[SerializeField] private Button ContinueButton;
		[SerializeField] private TextMeshProUGUI GameResultText;
		[SerializeField] private Sprite GameWonSprite;
		[SerializeField] private Sprite GameLostSprite;
		[SerializeField] private Image GameResultImage;
		[SerializeField] private PlayableDirector Timeline;
		[SerializeField] private PlayableAsset GameWonPlayableAsset;
		[SerializeField] private PlayableAsset GameLostPlayableAsset;

		public TextMeshProUGUI gameResultText => GameResultText;
		public Button continueButton => ContinueButton;
		public Sprite gameWonSprite => GameWonSprite;
		public Sprite gameLostSprite => GameLostSprite;
		public Image gameResultImage => GameResultImage;
		public PlayableDirector timeline => Timeline;

		public PlayableAsset gameWonPlayableAsset => GameWonPlayableAsset;

		public PlayableAsset gameLostPlayableAsset => GameLostPlayableAsset;
	}
}