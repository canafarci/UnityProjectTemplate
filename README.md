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

## Custom Object Pool

-------
The custom object pool in this project is designed to efficiently manage the creation and reuse of objects, both MonoBehaviours and pure C# classes, that implement the IPoolable interface. It helps reduce memory allocations and improve performance by reusing objects instead of instantiating and destroying them frequently.

### Key Features
* Supports MonoBehaviours and Pure C# Classes: Manage pools for both types of objects.
* Editor Configurable: Define pool settings directly in the Unity Editor via PoolConfig.
* Lifecycle Management: Objects implement IPoolable to handle their own initialization and cleanup.
* Scene-Based Pool Management: Optionally manage pools based on scene changes using AppStateID.
* Thread-Safe Operations: Safe to use in asynchronous contexts.

### Setting Up the PoolManager
To use the object pool, you need to register the PoolManager in your dependency injection container (e.g., VContainer) and provide it with a PoolConfig.

Registering PoolManager:
```

public class ProjectLifetimeScope : LifetimeScope
{
    [SerializeField] private PoolConfig PoolConfig;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterPoolManager(PoolConfig);
    }
}
```

---- 

### Configuring Pools via PoolConfig
PoolConfig is a ScriptableObject that holds a list of PoolEntry objects, each defining the settings for a specific pool.

#### Creating a PoolConfig:

```
Create a PoolConfig Asset:

Right-click in the Project window.
Select Create > Infrastructure > PoolConfig.
Name the asset (e.g., "MyPoolConfig").
```
Define Pool Entries:

* Open the PoolConfig asset.
* Add new PoolEntry items to the list.

#### PoolEntry Fields:

* IsMonoBehaviour: Check this if the object is a MonoBehaviour.
* Prefab: Assign the prefab if IsMonoBehaviour is true.
* ClassTypeName: Select the class type that implements IPoolable.
* InitialSize: Number of objects to instantiate initially.
* DefaultCapacity: Initial capacity of the pool's internal list.
* MaximumSize: Maximum number of objects the pool can hold.
* ManagePoolOnSceneChange: If true, the pool will manage objects based on scene changes.
* LifetimeSceneID: Specify the scene or app state the pool is associated with.

---- 

### Implementing the IPoolable Interface
Any object you want to pool must implement the IPoolable interface:
```
public interface IPoolable
{
    void OnCreated();
    void OnDestroyed();
    void OnGetFromPool();
    void OnReturnToPool();
}
```
### Example Implementation for a MonoBehaviour:
```
public class Enemy : MonoBehaviour, IPoolable
{
    public void OnCreated()
    {
        // Initialization logic when the object is first created
    }

    public void OnDestroyed()
    {
        // Cleanup logic before the object is destroyed
    }

    public void OnGetFromPool()
    {
        // Reset the object's state when retrieved from the pool
    }

    public void OnReturnToPool()
    {
        // Logic before the object is returned to the pool
    }
}
```
Example Implementation for a Pure C# Class:
```
public class Projectile : IPoolable
{
    public void OnCreated()
    {
    // Initialization logic
    }

    public void OnDestroyed()
    {
        // Cleanup logic
    }

    public void OnGetFromPool()
    {
        // Reset state
    }

    public void OnReturnToPool()
    {
        // Logic before returning to pool
    }
}
```
----
### Getting and Releasing Objects from the Pool
Use the PoolManager to get and release objects.

Injecting PoolManager:
```
public class EnemySpawner : MonoBehaviour
{
    [Inject] private PoolManager _poolManager;

    // ...
}
```

Getting a Pooled MonoBehaviour Object:
```
Enemy enemy = _poolManager.GetMono<Enemy>();
// Use the enemy instance
```
Releasing a MonoBehaviour Object Back to the Pool:
```
_poolManager.ReleaseMono(enemy);
```
Getting a Pooled Pure C# Object:

```
Projectile projectile = _poolManager.GetPure<Projectile>();
// Use the projectile instance
```
Releasing a Pure C# Object Back to the Pool:

```
_poolManager.ReleasePure(projectile);
```
Example Usage
Spawning Enemies:
```
public class EnemySpawner : MonoBehaviour
{
[Inject] private PoolManager _poolManager;

    private void SpawnEnemy()
    {
        Enemy enemy = _poolManager.GetMono<Enemy>();
        enemy.transform.position = GetSpawnPosition();
        // Initialize enemy as needed
    }

    private void DespawnEnemy(Enemy enemy)
    {
        _poolManager.ReleaseMono(enemy);
    }
}
```
Handling Projectiles:
```
public class Weapon : MonoBehaviour
{
    [Inject] private PoolManager _poolManager;

    public void Fire()
    {
        Projectile projectile = _poolManager.GetPure<Projectile>();
        projectile.SetPosition(transform.position);
        projectile.Launch();
    }

    private void OnProjectileExpired(Projectile projectile)
    {
        _poolManager.ReleasePure(projectile);
    }
}
```

-----
### Managing Pools on Scene Change
If ManagePoolOnSceneChange is enabled for a pool, the pool will automatically manage its objects based on the specified LifetimeSceneID.

Objects are destroyed when exiting the scene and Initial objects are instantiated when entering the scene. \
This ensures that the pool's objects are only available during the gameplay scene, helping to manage memory and resources efficiently.

### Best Practices
* Always Release Objects: Make sure to release objects back to the pool when done.
* Implement IPoolable Properly: Ensure all methods are appropriately handled to avoid unexpected behavior.
* Configure Pools Wisely: Set InitialSize, DefaultCapacity, and MaximumSize based on expected usage to optimize performance.
* Avoid Memory Leaks: If you're pooling MonoBehaviours that are not set to DontDestroyOnLoad, ensure they are properly managed when scenes change.
### Summary
* Register PoolManager with your container, providing a PoolConfig.
* Define Pools in PoolConfig via PoolEntry items.
* Implement IPoolable in your pooled objects.
* Use ```GetMono<T>()``` and ```ReleaseMono<T>()``` for MonoBehaviours.
* Use ```GetPure<T>()``` and ```ReleasePure<T>()``` for pure C# classes.
By following these steps, you can effectively utilize the custom object pool in your Unity project to enhance performance and manage resources efficiently.

Note: Ensure that any dependencies, such as Odin Inspector for the [FoldoutGroup] and [ValueDropdown] attributes, are included in your project to use the pool configuration in the editor.



### Acknowledgments
Special thanks to the developers of VContainer, UniTask, and the Unity community for their invaluable resources and tools.

For more information, visit the GitHub repository.

DEPENDENCIES: 

Odin Inspector \
ES3 \
DOTween \
Feel/NiceVibrations
