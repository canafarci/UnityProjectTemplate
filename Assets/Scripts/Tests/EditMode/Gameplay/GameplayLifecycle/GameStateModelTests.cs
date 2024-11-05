using Moq;
using NUnit.Framework;
using ProjectTemplate.Runtime.Gameplay.GameplayLifecycle.GameStates;
using ProjectTemplate.Runtime.Gameplay.Signals;
using ProjectTemplate.Runtime.Infrastructure.Signals;
using VContainer;

namespace ProjectTemplate.Tests.EditMode.Gameplay.GameplayLifecycle
{
	public class GameStateModelTests
	{
		private IObjectResolver _container;
		private SignalBus _signalBus;
		private GameStateController _gameStateController;
		private GameStateModel _gameStateModel;

		[SetUp]
		public void SetUp()
		{
			// Initialize the SignalBus and the container
			ContainerBuilder builder = new();

			// Register mock and dependencies in VContainer
			builder.Register<IGameStateModel, GameStateModel>(Lifetime.Scoped);
			builder.Register<GameStateController>(Lifetime.Singleton);

			builder.RegisterSignalBus();

			builder.DeclareSignal<ChangeGameStateSignal>();
			builder.DeclareSignal<SetGameResultSignal>();
			builder.DeclareSignal<GameStateChangedSignal>();


			_container = builder.Build();

			_signalBus = _container.Resolve<SignalBus>();
			_gameStateController = _container.Resolve<GameStateController>();
			_gameStateModel = _container.Resolve<IGameStateModel>() as GameStateModel; 

			// Initialize SignalListener
			_gameStateController.Initialize();
		}
		
		[TearDown]
		public void TearDown()
		{
			_gameStateController.Dispose();
			_container.Dispose();
		}

		[Test]
		public void SetGameResultSignal_OnGameWon_Should_Set_IsGameWon_True()
		{
			//Act
			_signalBus.Fire(new SetGameResultSignal(isGameWon: true));
			//Assert
			Assert.IsTrue(_gameStateModel.isGameWon);
		}
		
		[Test]
		public void SetGameResultSignal_OnGameLost_Should_Set_IsGameWon_False()
		{
			//Act
			_signalBus.Fire(new SetGameResultSignal(isGameWon: false));
			//Assert
			Assert.IsFalse(_gameStateModel.isGameWon);
		}
	}
}