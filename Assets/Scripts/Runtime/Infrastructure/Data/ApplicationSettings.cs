using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ProjectTemplate.Runtime.Infrastructure.Data
{
	[CreateAssetMenu(fileName = "Application Settings", menuName = "Infrastructure/Application Settings", order = 0)]
	public class ApplicationSettings : SerializedScriptableObject
	{
		public int TargetFrameRate = 60;
		public bool ShowLoadingScreen = false;
		public bool HasMainMenu = false;
		
		[TitleGroup("Scene Indexes")]
		
		public int FirstGameplayLevelIndex;
		public int LevelToLoopAfterAllLevelsFinishedIndex;
		[ShowIf("HasMainMenu")]
		public int MainMenuSceneIndex;
		
		[ShowIf("ShowLoadingScreen")]
		[TitleGroup("Loading Screen")]
		
		[ShowIf("ShowLoadingScreen")]
		public float LoadingScreenFadeDuration = 0.5f;
		[ShowIf("ShowLoadingScreen")]
		public GameObject LoadingScreenPrefab;
		
		[TitleGroup("DOTween Settings")]
		public bool RecycleAllByDefault = false;
		public bool UseSafeMode = false;
		public Ease DefaultEase = Ease.Linear;
		public int TweenCapacity;
		public int SequenceCapacity;
		
	}
}