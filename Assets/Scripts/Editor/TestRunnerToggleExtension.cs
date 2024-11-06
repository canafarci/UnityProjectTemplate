using UnityEditor;
using UnityEditor.TestTools.TestRunner;
using UnityEngine;
using UnityEngine.UIElements;

namespace ProjectTemplate.Editor
{
    [InitializeOnLoad]
    public class TestRunnerToggleExtension
    {
        private const string IS_TESTING_KEY = "IsTestingPlayMode";
        private static bool customToggle;

        static TestRunnerToggleExtension()
        {
            // Initialize toggle state based on saved preference
            customToggle = EditorPrefs.HasKey(IS_TESTING_KEY);
        
            // Listen to the editor update to inject the toggle when Test Runner is open
            EditorApplication.update += InjectToggleIntoTestRunnerWindow;
        }

        private static void InjectToggleIntoTestRunnerWindow()
        {
            // Find and open the Test Runner window if it's not already open
            var testRunnerWindowType = typeof(TestRunnerWindow);
            var testRunnerWindow = EditorWindow.GetWindow(testRunnerWindowType, false, "Test Runner", false);

            if (testRunnerWindow != null)
            {
                // Attach IMGUIContainer for custom GUI
                var root = testRunnerWindow.rootVisualElement;

                // Check if toggle already exists to avoid multiple injections
                if (root.Q<IMGUIContainer>("CustomToggleContainer") == null)
                {
                    // Create IMGUIContainer to hold the toggle UI
                    var container = new IMGUIContainer(() =>
                                                       {
                                                           EditorGUILayout.BeginVertical();

                                                           // Add the toggle with a label and tooltip
                                                           var labelContent = new GUIContent(
                                                                "Disable Bootstrap Loading",
                                                                "Bootstrap scene loading should be disabled when running play mode tests!"
                                                               );
                                                           customToggle = EditorGUILayout.ToggleLeft(labelContent, customToggle);

                                                           // Update EditorPrefs based on toggle state
                                                           if (customToggle)
                                                           {
                                                               EditorPrefs.SetBool(IS_TESTING_KEY, true);
                                                           }
                                                           else
                                                           {
                                                               EditorPrefs.DeleteKey(IS_TESTING_KEY);
                                                           }

                                                           EditorGUILayout.EndVertical();
                                                       })
                                    {
                                        name = "CustomToggleContainer" // Set name for reference
                                    };

                    // Add the container to the bottom of the Test Runner window
                    root.Add(container);
                
                    // Unsubscribe after successful injection
                    EditorApplication.update -= InjectToggleIntoTestRunnerWindow;
                }
            }
        }
    }
}