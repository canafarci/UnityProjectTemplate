# Unity Project Template

This repository provides a foundational template for Unity projects, integrating several key components to streamline development.

## Features
**<ins>Dependency Injection with VContainer</ins>**: Utilizes VContainer for efficient and scalable dependency injection. \
**<ins>Asynchronous Operations with UniTask</ins>**: Incorporates UniTask to handle asynchronous programming patterns seamlessly. \
**<ins>Scene Scope Architecture </ins>**: Implements a structured approach with Bootstrap and Level scenes, facilitating organized initialization and scene management. \
**<ins> Signal Bus </ins>**: Centralized event system inspired from Zenject's SignalBus, which does not exist inside VContainer directly. \
**<ins> Editor Configurable Object Pool </ins>**: An object pool directly configurable from the editor, with support for both Pure C# and Mono classes implementing IPoolable interface \
**<ins> Bootstrap Scene Loader </ins>**: An editor tool to load bootstrap scene every time Play mode is entered, with option to load currently loaded scene after bootstrapping as well

## Architecture Overview
The project follows a Bootstrap initialization pattern leading to Level scene loading. An optional Main Menu layer can be inserted between the Bootstrap and Level scenes, allowing the Main Menu to manage scene navigation and settings.

Getting Started

Clone the Repository:
git clone https://github.com/canafarci/UnityProjectTemplate.git

Acknowledgments
Special thanks to the developers of VContainer, UniTask, and the Unity community for their invaluable resources and tools.

For more information, visit the GitHub repository.

DEPENDENCIES: 

Odin Inspector \
ES3 \
DOTween \
Feel/NiceVibrations
