using System;
using System.Collections.Generic;
using System.Linq;
using ProjectTemplate.Runtime.Infrastructure.ApplicationState;
using ProjectTemplate.Runtime.Infrastructure.MemoryPool.Templates;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ProjectTemplate.Runtime.Infrastructure.MemoryPool
{
	[Serializable]
	public class PoolEntry
	{
		public bool IsMonoBehaviour;

		[ShowIf(nameof(IsMonoBehaviour))]
		public GameObject Prefab;

		// store the class type as a string for serialization
		[ValueDropdown("GetClassTypeNames")]
		[SerializeField]
		private string ClassTypeName;
		// Start Size of the Pool
		public int InitialSize = 5;
		//Start Size of the Pool reference list
		public int DefaultCapacity = 10;
		public int MaximumSize = 100;
		//Whether the Pool objects should be only instantiated and removed in regard to application entering a state
		public bool ManagePoolOnSceneChange = true;
		
		[ShowIf(nameof(ManagePoolOnSceneChange))]
		// App state which pool objects should be spawned upon entering and removed when exiting
		public AppStateID LifetimeSceneID;    

		// property to get the actual Type from the string
		public Type classType
		{
			get => Type.GetType(ClassTypeName);
			set => ClassTypeName = value?.Name;
		}

		// odin dropdown to show available classes implemeting IPoolable classes
		private static IEnumerable<ValueDropdownItem<string>> GetClassTypeNames()
		{
			var types = AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany(assembly => assembly.GetTypes())
				.Where(t => t.IsClass && !t.IsAbstract &&
				            (typeof(IPoolable).IsAssignableFrom(t) ||
				             (t.BaseType != null && typeof(Poolable).IsAssignableFrom(t.BaseType)) || 
				             (t.BaseType != null && typeof(MonoPoolable).IsAssignableFrom(t.BaseType))))
				.Select(type => new ValueDropdownItem<string>(type.Name, type.AssemblyQualifiedName));


			return types;
		}
	}
}