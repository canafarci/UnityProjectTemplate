namespace ProjectTemplate.Gameplay.GameplayLifecycle.GameStates
{
	public class GameStateModel : IGameStateModel
	{
		public bool isGameWon { get; private set; }
		
		public void SetGameIsWon(bool gameIsWon)
		{
			isGameWon = gameIsWon;
		}
	}
}