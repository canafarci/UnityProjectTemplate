using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using ProjectTemplate.Runtime.Gameplay.GameplayLifecycle.Enums;
using ProjectTemplate.Runtime.Gameplay.GameplayLifecycle.Signals;

namespace ProjectTemplate.Runtime.Infrastructure.Templates
{
	/// <summary>
	/// Class responsible for initializing relevant game modules.
	/// Signal InitializeGameSignal is listened by any module that needs initialization.
	/// when they start initializing, they send signal <see cref="ModuleInitializedSignal"/>> which is listened from this class.
	/// </summary>
	public abstract class SceneInitializer<T> : SignalListener  where T : Enum
	{
		protected readonly HashSet<T> _initializedModules = new();

		internal async UniTask InitializeModules()
		{
			_signalBus.Fire(new InitializeModulesSignal());

			int moduleCount = Enum.GetValues(typeof(T)).Length;

			while (moduleCount > 0 && _initializedModules.Count != moduleCount)
			{
				await UniTask.NextFrame();
			}
			
			_signalBus.Fire(new AllModulesInitializedSignal());
		}
		
		protected override void SubscribeToEvents()
		{
			_signalBus.Subscribe<ModuleInitializedSignal<T>>(OnModuleInitializedSignalHandler);
		}

		private void OnModuleInitializedSignalHandler(ModuleInitializedSignal<T> signal) => _initializedModules.Add(signal.initializableModule);

		protected override void UnsubscribeFromEvents()
		{
			_signalBus.Unsubscribe<ModuleInitializedSignal<T>>(OnModuleInitializedSignalHandler);
		}
	}
}