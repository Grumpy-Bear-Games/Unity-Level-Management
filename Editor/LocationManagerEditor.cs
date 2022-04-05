using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Games.GrumpyBear.LevelManagement.Editor
{
    [CustomEditor(typeof(LocationManager))]
    public class LocationManagerEditor: UnityEditor.Editor
    {
        private LocationManager _locationManager;

        private void OnEnable()
        {
            _locationManager = target as LocationManager;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var hasProblems = false;
            foreach (var scene in _locationManager.GlobalScenes)
            {
                if (string.IsNullOrEmpty(scene.ScenePath)) continue;
                if (scene.BuildIndex != -1) continue;
                EditorGUILayout.HelpBox($"{scene.ScenePath} is missing from the build", MessageType.Warning);
                hasProblems = true;
            }
            
            if (hasProblems && GUILayout.Button("Fix all problems")) FixAllProblems();
        }
        
        private void FixAllProblems()
        {
            var editorBuildSettingsScenes = EditorBuildSettings.scenes.ToList();
            editorBuildSettingsScenes.AddRange(
                from scene in _locationManager.GlobalScenes
                where scene.BuildIndex == -1
                select new EditorBuildSettingsScene(scene.ScenePath, true)
            );
            EditorBuildSettings.scenes = editorBuildSettingsScenes.ToArray();
        }
       
    }
}
