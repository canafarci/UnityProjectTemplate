using ProjectTemplate.Runtime.Gameplay.GameplayLifecycle.Enums;

namespace ProjectTemplate.Runtime.Gameplay.GameplayLifecycle.Signals
{
	public struct ModuleInitializedSignal
	{
		public InitializableModule initializableModule { get; }

		public ModuleInitializedSignal(InitializableModule initializableModule)
		{
			this.initializableModule = initializableModule;
		}
	}
}