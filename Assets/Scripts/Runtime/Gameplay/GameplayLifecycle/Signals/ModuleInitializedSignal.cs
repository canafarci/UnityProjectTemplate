using System;
using ProjectTemplate.Runtime.Gameplay.GameplayLifecycle.Enums;

namespace ProjectTemplate.Runtime.Gameplay.GameplayLifecycle.Signals
{
	public struct ModuleInitializedSignal<T> where T : Enum
	{
		public T initializableModule { get; }

		public ModuleInitializedSignal(T initializableModule)
		{
			this.initializableModule = initializableModule;
		}
	}
}