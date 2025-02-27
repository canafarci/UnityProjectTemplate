using ProjectTemplate.Runtime.CrossScene.Data;
using VContainer.Unity;

namespace ProjectTemplate.Runtime.Gameplay.UI.GameplayLevelDisplayPanel
{
	public class GameplayLevelDisplayController : IInitializable
	{
		private readonly GameplayLevelDisplayView _view;
		private readonly IGameplayPersistentData _gameplayPersistentData;

		public GameplayLevelDisplayController(GameplayLevelDisplayView view, IGameplayPersistentData gameplayPersistentData)
		{
			_view = view;
			_gameplayPersistentData = gameplayPersistentData;
		}

		public void Initialize()
		{
			_view.levelText.text = $"Level {_gameplayPersistentData.levelVisualDisplayNumber}";
		}
	}
}