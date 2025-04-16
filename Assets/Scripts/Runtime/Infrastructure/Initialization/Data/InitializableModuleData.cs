using System.Collections.Generic;
using ProjectTemplate.Runtime.Infrastructure.Initialization.Enums;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ProjectTemplate.Runtime.Infrastructure.Initialization.Data
{
	[CreateAssetMenu(fileName = "Initializable Module Data", menuName = "Infrastructure/Initializable Module Data", order = 0)]
	public class InitializableModuleData : SerializedScriptableObject
	{
		[SerializeField] private List<InitializableModuleSpec> InitializableModuleSpecs;

		public List<InitializableModuleSpec> initializableModuleSpecs => InitializableModuleSpecs;
	}

	public struct InitializableModuleSpec
	{
		public ModuleDomain ModuleDomain;
		public InitializableModule ModuleID;
		public int Priority;
	}
}