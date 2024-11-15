using System;
using Cysharp.Threading.Tasks;
using ProjectTemplate.Runtime.Gameplay.Enums;
using ProjectTemplate.Runtime.Gameplay.Signals;
using ProjectTemplate.Runtime.Infrastructure.ApplicationState;
using ProjectTemplate.Runtime.Infrastructure.ApplicationState.Signals;
using ProjectTemplate.Runtime.Infrastructure.Templates;

namespace ProjectTemplate.Runtime.Gameplay.GameplayLifecycle
{
	public class GameplayEntryPoint : SceneEntryPoint
	{
		protected override async void EnterScene()
		{
			try
			{
				_signalBus.Fire(new ChangeAppStateSignal(AppStateID.Gameplay));
				_signalBus.Fire(new ChangeGameStateSignal(GameState.Initializing));
			
				InitializeGameplay();

				await UniTask.Delay(TimeSpan.FromSeconds(1f));
			
				_signalBus.Fire(new ChangeGameStateSignal(GameState.Playing));
			}
			catch (Exception e)
			{
				UnityEngine.Debug.LogWarning(e);
			}
		}
		
		private void InitializeGameplay()
		{
		}
	}
}