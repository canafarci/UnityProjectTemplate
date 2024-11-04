using UnityEditor;
using UnityEngine;
using System.IO;
using System.Text;

namespace ProjectTemplate.Infrastructure.Pool.Editor
{
	[CustomEditor(typeof(PoolConfig))]
	public class PoolConfigEditor : UnityEditor.Editor
	{
		private const string EnumFilePath = "Assets/Scripts/PoolID.cs"; // Adjust path as needed

		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();

			if (GUILayout.Button("Update PoolID Enum")) UpdateEnumFile();
		}

		private void UpdateEnumFile()
		{
			PoolConfig poolConfig = (PoolConfig)target;
			StringBuilder enumBuilder = new();

			// Start creating the enum structure
			enumBuilder.AppendLine("public enum PoolID");
			enumBuilder.AppendLine("{");

			foreach (PoolConfig.PoolEntry entry in poolConfig.PoolEntries)
			{
				string enumEntry = ConvertToValidEnumName(entry.PoolID);
				enumBuilder.AppendLine($"    {enumEntry},");
			}

			enumBuilder.AppendLine("}");

			// Write to file
			File.WriteAllText(EnumFilePath, enumBuilder.ToString());
			AssetDatabase.Refresh();

			Debug.Log("PoolID enum updated successfully.");
		}

		private string ConvertToValidEnumName(string input) => input.Replace(" ", "_").Replace("-", "_");
	}
}