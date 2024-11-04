using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace ProjectTemplate.Infrastructure.Pool
{
	public class PoolEnumFileUpdater : IDisposable
	{
		private const string ENUM_FILE_PATH = "Assets/Scripts/Infrastructure/Pool/Enums/PoolID.cs";

		public void UpdateEnumFile(List<PoolEntry> poolEntries)
		{
			if (poolEntries == null || !poolEntries.Any())
			{
				Debug.LogWarning("No PoolEntries provided. Update aborted.");
				return;
			}

			// Read and parse the existing file
			List<string> lines = ReadEnumFile();
			List<string> existingFields = ExtractEnumFields(lines, out int startIdx, out int endIdx);

			// Get desired fields and check for necessary changes
			List<string> desiredFields = poolEntries.Select(entry => ConvertToValidEnumName(entry.PoolID)).ToList();
			if (existingFields.SequenceEqual(desiredFields))
			{
				Debug.Log("No changes detected in PoolID enum. Update skipped.");
				return;
			}

			// Generate updated content and write it to the file
			List<string> updatedLines = GenerateUpdatedEnumContent(lines, desiredFields, startIdx, endIdx);
			WriteEnumFile(updatedLines);

			Debug.Log("PoolID enum updated successfully.");
		}

		private List<string> ReadEnumFile() => File.Exists(ENUM_FILE_PATH) ? File.ReadAllLines(ENUM_FILE_PATH).ToList() : new List<string>();

		private List<string> ExtractEnumFields(List<string> lines, out int startIdx, out int endIdx)
		{
			const string enumStartMarker = "public enum PoolID";
			const string enumEndMarker = "}";
			List<string> existingFields = new();

			startIdx = -1;
			endIdx = -1;
			bool insideEnum = false;

			for (int i = 0; i < lines.Count; i++)
			{
				string line = lines[i].Trim();

				if (line.StartsWith(enumStartMarker))
				{
					insideEnum = true;
					startIdx = i;
				}
				else if (insideEnum && line == enumEndMarker)
				{
					insideEnum = false;
					endIdx = i;
					break;
				}
				else if (insideEnum)
				{
					string field = line.Trim().TrimEnd(',');
					if (!string.IsNullOrEmpty(field)) existingFields.Add(field);
				}
			}

			return existingFields;
		}

		private List<string> GenerateUpdatedEnumContent(List<string> lines,
		                                                List<string> desiredFields,
		                                                int startIdx,
		                                                int endIdx)
		{
			List<string> updatedEnumSection = new() { "\tpublic enum PoolID", "\t{" };
			updatedEnumSection.AddRange(desiredFields.Select(field => $"\t\t{field},"));
			updatedEnumSection.Add("\t}");

			// If the enum exists, replace it; otherwise, append it to the end of the file
			if (startIdx != -1 && endIdx != -1)
			{
				lines.RemoveRange(startIdx, endIdx - startIdx + 1);
				lines.InsertRange(startIdx, updatedEnumSection);
			}
			else
			{
				lines.AddRange(updatedEnumSection);
			}

			return lines;
		}

		private void WriteEnumFile(List<string> lines)
		{
			File.WriteAllText(ENUM_FILE_PATH, string.Join(Environment.NewLine, lines));
			AssetDatabase.Refresh();
		}

		private string ConvertToValidEnumName(string input) => input.Replace(" ", "_").Replace("-", "_");

		public void Dispose()
		{
			// No explicit resources to release in this implementation, but setup for future needs.
			GC.SuppressFinalize(this);
		}
	}
}