using System.Collections.Generic;
using ProjectTemplate.Runtime.CrossScene.Enums;
using ProjectTemplate.Runtime.Infrastructure.EnumFieldAdder;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ProjectTemplate.Runtime.CrossScene.Progress
{
	[CreateAssetMenu(fileName = "Progress Data", menuName = "Infrastructure/Progress Data")]
	public class ProgressData : SerializedScriptableObject
	{
		[SerializeField] private List<ProgressStep> ProgressSteps = new ();
		
		public List<ProgressStep> progressSteps => ProgressSteps;
		
		[Space(100f)]
		[InfoBox("You can use the input field below to add new Unlockable IDs")]
		[ShowInInspector] private EnumFieldAdder<UnlockableID> _unlockableIDAdder = new();
	}

	public struct ProgressStep
	{
		public Sprite Icon;
		public int LevelsToCompleteToUnlock;
		public UnlockableID UnlockableID;
		public string Name;
	}
}