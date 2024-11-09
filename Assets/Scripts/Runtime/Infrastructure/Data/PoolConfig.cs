using System.Collections.Generic;
using ProjectTemplate.Runtime.Infrastructure.MemoryPool;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ProjectTemplate.Runtime.Infrastructure.Data
{
	[CreateAssetMenu(fileName = "Pool Config", menuName = "Infrastructure/Pool Config")]
	public class PoolConfig : SerializedScriptableObject
	{
		public List<PoolEntry> PoolEntries = new();
	}
}