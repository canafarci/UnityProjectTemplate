using UnityEngine;

using Sirenix.OdinInspector;

using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectTemplate.Infrastructure.Pool
{
	[CreateAssetMenu(fileName = "PoolConfig", menuName = "Infrastructure/PoolConfig")]
	public class PoolConfig : SerializedScriptableObject
	{
		[Serializable]
		public class PoolEntry
		{
			[HorizontalGroup("ID")]
			[HideLabel]
			public string PoolID;

			[HorizontalGroup("Prefab")]
			[HideLabel]
			[ShowIf("@this.isMonoBehaviour")]
			public GameObject Prefab;

			[HorizontalGroup("Class")]
			[HideLabel]
			[ShowIf("@!this.isMonoBehaviour")]
			[ValueDropdown("GetClassTypes")]
			public Type ClassType;

			[HorizontalGroup("IsMono")]
			[HideLabel]
			public bool IsMonoBehaviour;

			// Odin Inspector dropdown for selecting available classes
			private static IEnumerable<Type> GetClassTypes()
			{
				var types = new List<Type>();
				foreach (Type type in AppDomain.CurrentDomain.GetAssemblies()
				                               .SelectMany(assembly => assembly.GetTypes())
				                               .Where(t => t.IsClass && !t.IsAbstract))
				{
					types.Add(type);
				}
				return types;
			}
		}

		[TableList]
		public List<PoolEntry> PoolEntries = new List<PoolEntry>();
	}
}