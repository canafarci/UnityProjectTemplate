using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;

namespace ProjectTemplate.Infrastructure.Pool
{
	[CreateAssetMenu(fileName = "PoolConfig", menuName = "Infrastructure/PoolConfig")]
	public class PoolConfig : SerializedScriptableObject
	{
		[TableList]
		public List<PoolEntry> PoolEntries = new();
	}
}