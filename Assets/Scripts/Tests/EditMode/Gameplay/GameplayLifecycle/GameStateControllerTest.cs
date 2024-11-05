using System.ComponentModel;
using System.Reflection;
using Moq;
using NUnit.Framework;
using ProjectTemplate.Runtime.Gameplay.GameplayLifecycle.GameStates;
using ProjectTemplate.Runtime.Gameplay.Enums;
using ProjectTemplate.Runtime.Gameplay.Signals;
using ProjectTemplate.Runtime.Infrastructure.Signals;
using VContainer;

namespace ProjectTemplate.Tests.EditMode.Gameplay.GameplayLifecycle
{
	internal class GameStateControllerTests
	{
		private SignalBus _signalBus;
		private Mock<IGameStateModel> _mockGameStateModel;
		private GameStateController _gameStateController;
		private IObjectResolver _container;

		[SetUp]
		public void SetUp()
		{
			// Initialize the SignalBus and the container
			ContainerBuilder builder = new();
			_mockGameStateModel = new Mock<IGameStateModel>();

			// Register mock and dependencies in VContainer
			builder.RegisterInstance(_mockGameStateModel.Object);
			builder.Register<GameStateController>(Lifetime.Singleton);

			builder.RegisterSignalBus();

			builder.DeclareSignal<ChangeGameStateSignal>();
			builder.DeclareSignal<SetGameResultSignal>();
			builder.DeclareSignal<GameStateChangedSignal>();


			_container = builder.Build();

			_signalBus = _container.Resolve<SignalBus>();
			_gameStateController = _container.Resolve<GameStateController>();

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
		public void Should_Subscribe_To_ChangeGameStateSignal_And_SetGameResultSignal_On_Initialize()
		{
			// Arrange
			ChangeGameStateSignal changeGameStateSignal = new(GameState.Playing);
			SetGameResultSignal setGameResultSignal = new (true);

			// Act
			_signalBus.Fire(changeGameStateSignal);
			_signalBus.Fire(setGameResultSignal);

			// Assert that both events are handled correctly
			_mockGameStateModel.Verify(m => m.SetGameIsWon(true), Times.Once);
		}

		
		[Test]
		public void Should_Fire_GameStateChangedSignal_When_GameState_Changes()
		{
		    //Arrange
		    GameState oldState = GameState.Initializing;
		    GameState newState = GameState.Playing;

		    FieldInfo currentStateField = _gameStateController.GetType()
		                                                      .GetField("_currentState",
																	BindingFlags.NonPublic | BindingFlags.Instance);
		    currentStateField.SetValue(_gameStateController, GameState.Initializing);
		    
		    bool signalFired = false;
		    
		    _signalBus.Subscribe<GameStateChangedSignal>(Handler);
		    // Subscribe to GameStateChangedSignal for verification
		    void Handler(GameStateChangedSignal signal)
		    {
			    signalFired = signal.newState == newState && signal.oldState == oldState;
		    }
		
		    // Act
		    _signalBus.Fire(new ChangeGameStateSignal(newState));
		
		    // Assert
		    Assert.IsTrue(signalFired, "GameStateChangedSignal was not fired correctly.");
		}

		[Test]
		public void Should_Set_Game_Result_When_SetGameResultSignal_Is_Received()
		{
		    // Arrange
		    bool isGameWon = true;
		
		    // Act
		    _signalBus.Fire(new SetGameResultSignal(isGameWon));
		
		    // Assert
		    _mockGameStateModel.Verify(m => m.SetGameIsWon(isGameWon), Times.Once);
		}
		
		[Test]
		public void Should_Unsubscribe_From_Events_On_Dispose()
		{
		    // Act
		    _gameStateController.Dispose();
		
		    // Attempt to fire signals after disposal
		    _signalBus.Fire(new ChangeGameStateSignal(GameState.Playing));
		    _signalBus.Fire(new SetGameResultSignal(true));
		
		    // Assert that the methods were not called after disposal
		    _mockGameStateModel.Verify(m => m.SetGameIsWon(It.IsAny<bool>()), Times.Never);
		}
	}
}