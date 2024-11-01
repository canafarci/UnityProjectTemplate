namespace ProjectTemplate.Gameplay.GameplayLifecycle.GameStates
{
	public interface IGameStateModel
	{
		public bool isGameWon { get; }
		public void SetGameIsWon(bool isGameWon);
	}
}