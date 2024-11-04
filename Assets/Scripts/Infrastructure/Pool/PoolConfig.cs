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

		[Button(ButtonSizes.Large)] 
		[GUIColor(0.4f, 0.8f, 1.0f)]
		private void UpdatePoolIDEnum()
		{
			PoolEnumFileUpdater fileEnumFileUpdater = new PoolEnumFileUpdater();
			
			using (fileEnumFileUpdater)
			{
				fileEnumFileUpdater.UpdateEnumFile(PoolEntries);
			}
		}
	}
}