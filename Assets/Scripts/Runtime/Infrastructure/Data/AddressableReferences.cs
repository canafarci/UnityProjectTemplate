using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace ProjectTemplate.Runtime.Infrastructure.Data
{
	[CreateAssetMenu(fileName = "Addressable References", menuName = "Infrastructure/Addressable References", order = 0)]
	public class AddressableReferences : SerializedScriptableObject
	{
		[SerializeField] private AssetReference MainMenuScene;
		[SerializeField] private AssetReference LevelToLoopAfterLevelsFinished;
		[SerializeField] private List<AssetReference> GameplayScenes;

		public AssetReference mainMenuScene => MainMenuScene;
		public AssetReference levelToLoopAfterLevelsFinished => LevelToLoopAfterLevelsFinished;
		public List<AssetReference> gameplayScenes => GameplayScenes;
	}
}