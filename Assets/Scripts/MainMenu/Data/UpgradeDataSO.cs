using System.Collections.Generic;
using MainMenu.Enums;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MainMenu.Data
{
	[CreateAssetMenu(fileName = "Upgrade Data", menuName = "WarRush/Upgrade Data", order = 0)]
	public class UpgradeDataSO : SerializedScriptableObject
	{
		[SerializeField] private Dictionary<UpgradeID, List<int>> UpgradesLookup;
		public Dictionary<UpgradeID, List<int>> upgradesLookup => UpgradesLookup;
	}
}