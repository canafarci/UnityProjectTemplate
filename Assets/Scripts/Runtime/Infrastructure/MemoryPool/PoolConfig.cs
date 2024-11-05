using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ProjectTemplate.Runtime.Infrastructure.MemoryPool
{
	[CreateAssetMenu(fileName = "PoolConfig", menuName = "Infrastructure/PoolConfig")]
	public class PoolConfig : SerializedScriptableObject
	{
		[TableList]
		public List<PoolEntry> PoolEntries = new();
	}
}