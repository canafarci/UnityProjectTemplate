namespace ProjectTemplate.Runtime.CrossScene.Data
{
	public interface IGameplayPersistentData
	{
		int levelToLoadIndex 
		{
			get; 
#if UNITY_EDITOR
			set;
#endif			
		}
		int levelVisualDisplayNumber { get; }
		bool IsFirstTimePlaying();
		void IncreaseTargetSceneIndex();
	}
}