using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using ProjectTemplate.Runtime.Infrastructure.Initialization.Data;
using ProjectTemplate.Runtime.Infrastructure.Initialization.Enums;
using ProjectTemplate.Runtime.Infrastructure.Initialization.Signals;
using ProjectTemplate.Runtime.Infrastructure.Templates;
using Sirenix.Utilities;

namespace ProjectTemplate.Runtime.Infrastructure.Initialization
{
	/// <summary>
	/// Class responsible for initializing relevant game modules.
	/// Signal InitializeGameSignal is listened by any module that needs initialization.
	/// when they start initializing, they send signal <see cref="ModuleInitializedSignal"/>> which is listened from this class.
	/// </summary>
	public class SceneInitializer : SignalListener
	{
		private readonly InitializableModuleData _initializableModuleData;
		private HashSet<InitializableModule> _initializedModules;
		private readonly Dictionary<int, List<InitializableModule>> _modulesToInitialize = new();

		public SceneInitializer(InitializableModuleData initializableModuleData)
		{
			_initializableModuleData = initializableModuleData;
		}

		public async UniTask InitializeModules(ModuleDomain moduleDomain)
		{
			CreateModulesToInitializeLookup(moduleDomain);

			int priority = 0;
			List<InitializableModule> modulesWithHighestPriority = new ();
			int countOfRequiredInitializedModules = 0;
			
			while (_modulesToInitialize.Values.Count > 0 && _initializedModules.Count != _modulesToInitialize.Values.Count)
			{
				countOfRequiredInitializedModules += _modulesToInitialize[priority].Count;
				_modulesToInitialize[priority].ForEach(x => _signalBus.Fire(new InitializeModuleSignal(x)));

				while (countOfRequiredInitializedModules < _initializedModules.Count )
				{
					await UniTask.NextFrame();
				}
				
				priority++;
			}
			
			_signalBus.Fire(new AllModulesInitializedSignal());
		}

		private void CreateModulesToInitializeLookup(ModuleDomain moduleDomain)
		{
			_initializableModuleData.initializableModuleSpecs
				.Where(x => x.ModuleDomain == moduleDomain)
				.ForEach(x =>
				{
					if (_modulesToInitialize.ContainsKey(x.Priority))
					{
						_modulesToInitialize[x.Priority].Add(x.ModuleID);
					}
					else
					{
						_modulesToInitialize[x.Priority] = new() {x.ModuleID};
					}
				});
		}

		protected override void SubscribeToEvents()
		{
			_signalBus.Subscribe<ModuleInitializedSignal>(OnModuleInitializedSignalHandler);
		}
		
		private void OnModuleInitializedSignalHandler(ModuleInitializedSignal signal)
		{
			_initializedModules.Add(signal.moduleID);
		}
		
		protected override void UnsubscribeFromEvents()
		{
			_signalBus.Unsubscribe<ModuleInitializedSignal>(OnModuleInitializedSignalHandler);
		}
	}
}