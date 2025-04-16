using ProjectTemplate.Runtime.Infrastructure.Initialization.Enums;

namespace ProjectTemplate.Runtime.Infrastructure.Initialization.Signals
{
	public readonly struct InitializeModuleSignal
	{
		public InitializableModule initializableModule { get; }
		
		public InitializeModuleSignal(InitializableModule initializableModule)
		{
			this.initializableModule = initializableModule;
		}
	}
}