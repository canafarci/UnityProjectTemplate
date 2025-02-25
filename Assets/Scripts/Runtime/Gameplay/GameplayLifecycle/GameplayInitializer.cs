using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using ProjectTemplate.Runtime.Gameplay.GameplayLifecycle.Enums;
using ProjectTemplate.Runtime.Gameplay.GameplayLifecycle.Signals;
using ProjectTemplate.Runtime.Infrastructure.Templates;

namespace ProjectTemplate.Runtime.Gameplay.GameplayLifecycle
{
	/// <summary>
	/// Class responsible for initializing relevant game modules.
	/// Signal InitializeGameSignal is listened by any module that needs initialization.
	/// when they start initializing, they send signal <see cref="ModuleInitializedSignal"/>> which is listened from this class.
	/// </summary>
	public class GameplayInitializer : SignalListener
	{
		private HashSet<InitializableModule> _initializedModules = new();

		internal async UniTask InitializeModules()
		{
			_signalBus.Fire(new InitializeModulesSignal());

			int moduleCount = Enum.GetValues(typeof(InitializableModule)).Length;

			while (moduleCount > 0 && _initializedModules.Count != moduleCount)
			{
				await UniTask.NextFrame();
			}
		}

		protected override void SubscribeToEvents()
		{
			_signalBus.Subscribe<ModuleInitializedSignal>(OnModuleInitializedSignalHandler);
		}

		private void OnModuleInitializedSignalHandler(ModuleInitializedSignal signal) => _initializedModules.Add(signal.initializableModule);

		protected override void UnsubscribeFromEvents()
		{
			_signalBus.Unsubscribe<ModuleInitializedSignal>(OnModuleInitializedSignalHandler);
		}
	}
}