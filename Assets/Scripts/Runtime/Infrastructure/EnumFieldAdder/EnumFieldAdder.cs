using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace ZumaMatch.Runtime.Infrastructure.EnumFieldAdder
{
    [HideLabel]
    [HideReferenceObjectPicker]
	public class EnumFieldAdder<T>
	{
#if UNITY_EDITOR
		[Title("Add New Field to Enum")]
        public string NewFieldName;

        [Button("Add Enum Field")]
        public void AddCurrency()
        {
            if (string.IsNullOrEmpty(NewFieldName) || !ValidateFieldName(NewFieldName))
            {
                Debug.LogWarning("Field name cannot be empty or already exist inside enum.");
                return;
            }

            UpdateEnum(NewFieldName);

            // Clear the input field after adding
            NewFieldName = string.Empty;
        }

        private bool ValidateFieldName(string name)
        {
            // Check if the name is empty
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            // Check if the field already exists in enum file
            if (Enum.IsDefined(typeof(T), name))
            {
                return false;
            }

            return true;
        }

        private void UpdateEnum(string fieldName)
        {
            // Find the path to the enum file
            string enumFilePath = GetEnumFilePath(typeof(T));

            if (string.IsNullOrEmpty(enumFilePath))
            {
                Debug.LogError($"Could not find the {typeof(T)} enum file.");
                return;
            }

            // Read the enum file
            List<string> lines = File.ReadAllLines(enumFilePath).ToList();

            // Parse the existing enum values
            List<string> existingValues = ExtractEnumFields(lines, out int startIdx, out int endIdx);

            // Add the new field name to the list
            existingValues.Add(ConvertToValidEnumName(fieldName));

            // Generate updated enum content
            List<string> updatedLines = GenerateUpdatedEnumContent(lines, existingValues, startIdx, endIdx);

            // Write back to the enum file
            File.WriteAllText(enumFilePath, string.Join("\n", updatedLines));

            // Refresh AssetDatabase
            AssetDatabase.Refresh();

            Debug.Log($"Field '{fieldName}' added to {typeof(T)} enum successfully.");
        }

        private string GetEnumFilePath(Type enumType)
        {
            // Find the .cs asset that defines the enum
            string[] guids = AssetDatabase.FindAssets($"{enumType.Name} t:Script");
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);

                // Read the file to check if it contains the enum definition
                string fileContent = File.ReadAllText(path);
                if (fileContent.Contains($"public enum {enumType.Name}"))
                {
                    return path;
                }
            }

            return null;
        }

        private List<string> ExtractEnumFields(List<string> lines, out int startIdx, out int endIdx)
        {
            string enumStartMarker = $"public enum {typeof(T).Name}";
            const string enumEndMarker = "}";

            List<string> existingFields = new List<string>();
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
                    if (!string.IsNullOrEmpty(field) && field != "{")
                    {
                        existingFields.Add(field);
                    }
                }
            }

            return existingFields;
        }

        private List<string> GenerateUpdatedEnumContent(List<string> lines, List<string> desiredFields, int startIdx, int endIdx)
        {
            List<string> updatedEnumSection = new()
            {
                $"\tpublic enum {typeof(T).Name}",
                "\t{"
            };

            updatedEnumSection.AddRange(desiredFields.Select(field => $"\t\t{field},"));
            updatedEnumSection.Add("\t}");

            // Replace the old enum definition with the new one
            if (startIdx != -1 && endIdx != -1)
            {
                lines.RemoveRange(startIdx, endIdx - startIdx + 1);
                lines.InsertRange(startIdx, updatedEnumSection);
            }
            else
            {
                // Enum not found, append to the end
                lines.AddRange(updatedEnumSection);
            }

            return lines;
        }

        private string ConvertToValidEnumName(string input)
        {
            // Replace invalid characters and ensure the name is valid for an enum
            string validName = input.Replace(" ", "_").Replace("-", "_");
            validName = new string(validName.Where(c => char.IsLetterOrDigit(c) || c == '_').ToArray());

            // Ensure the first character is a letter or underscore
            if (!char.IsLetter(validName[0]) && validName[0] != '_')
            {
                validName = "_" + validName;
            }

            return validName;
        }
#endif
	}
}
