using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

namespace ProjectTemplate.Runtime.Gameplay.UI.GameOverPanel
{
	public class GameOverPanelView : MonoBehaviour
	{
		[SerializeField] private Button ContinueButton;
		[SerializeField] private TextMeshProUGUI GameResultText;
		[SerializeField] private TextMeshProUGUI ButtonText;
		[SerializeField] private TextMeshProUGUI WonLevelText;
		[SerializeField] private TextMeshProUGUI LostLevelText;
		[SerializeField] private Sprite GameWonSprite;
		[SerializeField] private Sprite GameLostSprite;
		[SerializeField] private GameObject GameWonSecondaryImage;
		[SerializeField] private GameObject GameLostSecondaryImage;
		[SerializeField] private Image GameResultImage;
		[SerializeField] private PlayableDirector Timeline;
		[SerializeField] private GameObject Particle;
		[SerializeField] private Image ProgressBarSlider;
		[SerializeField] private Image ProgressBarIcon;
		[SerializeField] private TextMeshProUGUI ProgressBarTitleText;
		[SerializeField] private TextMeshProUGUI ProgressionCountText;
		[SerializeField] private GameObject ProgressPanel;

		public TextMeshProUGUI gameResultText => GameResultText;
		public Button continueButton => ContinueButton;
		public Sprite gameWonSprite => GameWonSprite;
		public Sprite gameLostSprite => GameLostSprite;
		public Image gameResultImage => GameResultImage;
		public PlayableDirector timeline => Timeline;
		public TextMeshProUGUI buttonText => ButtonText;
		public TextMeshProUGUI wonLevelText => WonLevelText;
		public TextMeshProUGUI lostLevelText => LostLevelText;
		public GameObject particle => Particle;
		public GameObject gameWonSecondaryImage => GameWonSecondaryImage;
		public GameObject gameLostSecondaryImage => GameLostSecondaryImage;
		public Image progressBarSlider => ProgressBarSlider;
		public Image progressBarIcon => ProgressBarIcon;
		public TextMeshProUGUI progressBarTitleText => ProgressBarTitleText;
		public TextMeshProUGUI progressionCountText => ProgressionCountText;
		public GameObject progressPanel => ProgressPanel;
	}
}