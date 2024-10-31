namespace ProjectTemplate.CrossScene.Data
{
	public interface IGameplayPersistentData
	{
		int levelToLoadIndex { get; }
		int levelVisualDisplayNumber { get; }
		bool IsFirstTimePlaying();
		void IncreaseTargetSceneIndex();
	}
}