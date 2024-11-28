using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace ProjectTemplate.Editor
{
	public class ProjectEditorSettingsEditorWindow : OdinMenuEditorWindow
	{
		[MenuItem("Tools/Project Editor Settings")]
		private static void OpenWindow()
		{
			GetWindow<ProjectEditorSettingsEditorWindow>().Show();
		}

		protected override OdinMenuTree BuildMenuTree()
		{
			OdinMenuTree tree = new OdinMenuTree(supportsMultiSelect: true);

			tree.AddAllAssetsAtPath("Project Settings", "Data", typeof(ScriptableObject), true)
				.AddThumbnailIcons();
			
			return tree;
		}
	}
}