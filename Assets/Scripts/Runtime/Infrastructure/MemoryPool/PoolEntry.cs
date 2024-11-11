using System;
using System.Collections.Generic;
using System.Linq;
using ProjectTemplate.Runtime.Infrastructure.ApplicationState;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace ProjectTemplate.Runtime.Infrastructure.MemoryPool
{
	[Serializable]
	public class PoolEntry
	{
		[FoldoutGroup("Pool Entry Data")]
		public bool IsMonoBehaviour;

		[FoldoutGroup("Pool Entry Data")]
		[ShowIf(nameof(IsMonoBehaviour))]
		public GameObject Prefab;

		// Store the class type as a string for serialization
		[FoldoutGroup("Pool Entry Data")]
		[ValueDropdown("GetClassTypeNames")]
		[SerializeField]
		private string ClassTypeName;

		[FoldoutGroup("Pool Entry Data")]
		public int InitialSize = 5;
		[FoldoutGroup("Pool Entry Data")]
		public int DefaultCapacity = 10;
		[FoldoutGroup("Pool Entry Data")]
		public int MaximumSize = 100;
		[FoldoutGroup("Pool Entry Data")]
		public bool ManagePoolOnSceneChange = true;
		
		[FoldoutGroup("Pool Entry Data")]
		[ShowIf(nameof(ManagePoolOnSceneChange))]
		public AppStateID LifetimeSceneID;    

		// Property to get the actual Type from the string
		public Type classType
		{
			get => Type.GetType(ClassTypeName);
			set => ClassTypeName = value?.Name;
		}

		// Odin dropdown to show available classes
		private static IEnumerable<ValueDropdownItem<string>> GetClassTypeNames()
		{
			var types = AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany(assembly => assembly.GetTypes())
				.Where(t => t.IsClass && !t.IsAbstract && typeof(IPoolable).IsAssignableFrom(t))
				.Select(type => new ValueDropdownItem<string>(type.FullName, type.AssemblyQualifiedName));


			return types;
		}
	}
}