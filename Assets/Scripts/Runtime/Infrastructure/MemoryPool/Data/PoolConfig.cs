using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ProjectTemplate.Runtime.Infrastructure.MemoryPool.Data
{
	[CreateAssetMenu(fileName = "Pool Config", menuName = "Infrastructure/Pool Config")]
	public class PoolConfig : SerializedScriptableObject
	{
		public List<PoolEntry> PoolEntries = new();
	}
}