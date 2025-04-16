using ProjectTemplate.Runtime.Infrastructure.Initialization.Enums;

namespace ProjectTemplate.Runtime.Infrastructure.Initialization.Signals
{
	public struct ModuleInitializedSignal
	{
		public InitializableModule moduleID { get; }

		public ModuleInitializedSignal(InitializableModule moduleID)
		{
			this.moduleID = moduleID;
		}
	}
}