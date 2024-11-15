# Unity Project Template

This repository provides a foundational template for Unity projects, integrating several key components to streamline development.

Features
--------

**<ins>Dependency Injection with VContainer</ins>**: Utilizes VContainer for efficient and scalable dependency injection. \
**<ins>Asynchronous Operations with UniTask</ins>**: Incorporates UniTask to handle asynchronous programming patterns seamlessly. \
**<ins>Scene Scope Architecture </ins>**: Implements a structured approach with Bootstrap and Level scenes, facilitating organized initialization and scene management. \
**<ins> Signal Bus </ins>**: Centralized event system inspired from Zenject's SignalBus, which does not exist inside VContainer directly. \
**<ins> Editor Configurable Object Pool </ins>**: An object pool directly configurable from the editor, with support for both Pure C# and Mono classes implementing IPoolable interface \
**<ins> Bootstrap Scene Loader </ins>**: An editor tool to load bootstrap scene every time Play mode is entered, with option to load currently loaded scene after bootstrapping as well

### Getting Started

Clone the Repository:
```
git clone https://github.com/canafarci/UnityProjectTemplate.git
```
This template is developed using Unity version 2022.3.24f.

## Architecture Overview
The project follows a Bootstrap initialization pattern leading to Level scene loading. An optional Main Menu layer can be inserted between the Bootstrap and Level scenes, allowing the Main Menu to manage scene navigation and settings.

### Entry Points

Scene archetypes Main Menu and Gameplay Scenes each have an Entry Point class. This class can be used to initialize classes, load resources etc.

```
protected override void EnterScene()
    {
        _signalBus.Fire(new ChangeAppStateSignal(AppStateID.Gameplay));
        _signalBus.Fire(new ChangeGameStateSignal(GameState.Initializing));
        
        InitializeGameplay();
        
        _signalBus.Fire(new ChangeGameStateSignal(GameState.Playing));
    }
    
    private void InitializeGameplay()
    {
    }
}
```
### Exit Points
Scene archetypes Main Menu and Gameplay Scenes each have an Exit Point class to manage unloading and cleanup, manage next level indices and then load the next scene.
These classes listen to signals

```
TriggerExitGameplayLevelSignal
```
For Gameplay scene, this signal is triggered from GameStateController class when it processes 
```
TriggerLevelEndSignal(bool isGameWon)
```
which takes in a boolean to set game result in IGameStateModel class.

----

For main menu, exit point class listens for signal

```
TriggerExitMainMenuSignal
```

respectively

## Signal Bus


The Signal Bus is a centralized event system that enables decoupled communication between components in your Unity project. It allows you to declare, subscribe to, and fire signals dynamically at runtime, facilitating a clean and modular architecture.

Registering Signals in Scopes
Signals can be declared within any scope, such as different scenes or lifetime scopes, allowing for flexible and modular signal management.

During Container Setup:
```
public class SampleLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterSignalBus();
        builder.DeclareSignal<PlayerScoredSignal>();
    }
}
```

### Using the SignalListener Class
To simplify subscribing and unsubscribing from signals, you can inherit from the SignalListener abstract class. This ensures that subscriptions are managed correctly and reduces boilerplate code.

SignalListener Base Class:
```
using System;
using VContainer;
using VContainer.Unity;

public abstract class SignalListener : IInitializable, IDisposable
{
[Inject] protected SignalBus _signalBus;

    public virtual void Initialize()
    {
        SubscribeToEvents();
    }

    protected abstract void SubscribeToEvents();
    protected abstract void UnsubscribeFromEvents();

    public virtual void Dispose()
    {
        UnsubscribeFromEvents();
    }
}
```
Implementing a Signal Listener:
```
public class ScoreManager : SignalListener
{
    protected override void SubscribeToEvents()
    {
        _signalBus.Subscribe<PlayerScoredSignal>(OnPlayerScored);
    }

    protected override void UnsubscribeFromEvents()
    {
        _signalBus.Unsubscribe<PlayerScoredSignal>(OnPlayerScored);
    }

    private void OnPlayerScored(PlayerScoredSignal signal)
    {
        // Update the score based on signal.Points
    }
}
```
### Firing Signals
When an event occurs, you can fire a signal to notify all subscribers.
```
public class PlayerController : MonoBehaviour
{
[Inject] private SignalBus _signalBus;

    private void ScorePoint()
    {
        var signal = new PlayerScoredSignal { Points = 10 };
        _signalBus.Fire(signal);
    }
}
```
Signal Class Example:
```
public struct PlayerScoredSignal
{
    public int Points;
}
```
### Summary
**Declare Signals:** \
Use DeclareSignal<TSignal>() in the appropriate scope. \
**Subscribe and Unsubscribe:** \
Inherit from SignalListener and implement SubscribeToEvents() and UnsubscribeFromEvents(). \
**Fire Signals**:  
Use _signalBus.Fire(new TSignal()) to notify all subscribers.
By leveraging the Signal Bus and the SignalListener class, you can create a flexible and maintainable event-driven architecture that scales with your project's needs.

Note: Remember to register the SignalBus in your container:

```
builder.RegisterSignalBus();
```
And include any necessary dependencies in your project, such as VContainer, to ensure proper functionality.

-------

### Acknowledgments
Special thanks to the developers of VContainer, UniTask, and the Unity community for their invaluable resources and tools.

For more information, visit the GitHub repository.

DEPENDENCIES: 

Odin Inspector \
ES3 \
DOTween \
Feel/NiceVibrations
