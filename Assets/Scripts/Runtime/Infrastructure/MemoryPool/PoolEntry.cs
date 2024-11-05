using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ProjectTemplate.Runtime.Infrastructure.MemoryPool
{
	[Serializable]
	public class PoolEntry
	{
		[HorizontalGroup("IsMono")]
		[HideLabel]
		public bool IsMonoBehaviour;

		[HorizontalGroup("Prefab")]
		[HideLabel]
		[ShowIf("@this.IsMonoBehaviour")]
		public GameObject Prefab;

		// Store the class type as a string for serialization
		[HorizontalGroup("Class")]
		[HideLabel]
		[ValueDropdown("GetClassTypeNames")]
		[SerializeField]
		private string classTypeName;

		// Property to get the actual Type from the string
		public Type ClassType
		{
			get => Type.GetType(classTypeName);
			set => classTypeName = value?.Name;
		}

		// Odin dropdown to show available classes
		private static IEnumerable<ValueDropdownItem<string>> GetClassTypeNames()
		{
			var types = AppDomain.CurrentDomain.GetAssemblies()
			                     .Where(a => a.FullName.StartsWith("Assembly-CSharp"))
			                     .SelectMany(assembly => assembly.GetTypes())
			                     .Where(t => t.IsClass && !t.IsAbstract)
			                     .Select(type => new ValueDropdownItem<string>(type.FullName, type.AssemblyQualifiedName));

			return types;
		}
	}
}