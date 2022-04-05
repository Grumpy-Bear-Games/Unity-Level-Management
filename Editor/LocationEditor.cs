using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Games.GrumpyBear.LevelManagement.Editor
{
    [CustomEditor(typeof(Location))]
    public class LocationEditor: UnityEditor.Editor
    {
        private Location _location;
        private SerializedProperty _scenesProperty;
        
        private void OnEnable()
        {
            _location = target as Location;
            _scenesProperty = serializedObject.FindProperty("_scenes");
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox("The first scene in the list will be the active one.", MessageType.Info, true);

            base.OnInspectorGUI();

            var hasProblems = false;
            foreach (var scene in _location.Scenes)
            {
                if (string.IsNullOrEmpty(scene.ScenePath)) continue;
                if (scene.BuildIndex != -1) continue;
                EditorGUILayout.HelpBox($"{scene.ScenePath} is missing from the build", MessageType.Warning);
                hasProblems = true;
            }

            if (hasProblems && GUILayout.Button("Fix all problems")) FixAllProblems(); 

            GUI.enabled = _location.Scenes.Count > 0;
            if (GUILayout.Button("Load location")) LoadScene();
            GUI.enabled = true;
        }

        private void FixAllProblems()
        {
            var editorBuildSettingsScenes = EditorBuildSettings.scenes.ToList();
            editorBuildSettingsScenes.AddRange(
                from scene in _location.Scenes
                where scene.BuildIndex == -1
                select new EditorBuildSettingsScene(scene.ScenePath, true)
            );
            EditorBuildSettings.scenes = editorBuildSettingsScenes.ToArray();
        }

        private void LoadScene()
        {
            var openScenes = Enumerable.Range(0, EditorSceneManager.loadedSceneCount)
                .Select(EditorSceneManager.GetSceneAt)
                .Where(scene => _location.Scenes.All(x => scene.buildIndex != x.BuildIndex))
                .ToArray();
            if (!EditorSceneManager.SaveModifiedScenesIfUserWantsTo(openScenes)) return;
            foreach (var sceneAsset in _location.Scenes)
            {
                EditorSceneManager.OpenScene(sceneAsset.ScenePath, OpenSceneMode.Additive);
            }
            EditorSceneManager.SetActiveScene(EditorSceneManager.GetSceneByPath(_location.ActiveScene.ScenePath));
            foreach (var openScene in openScenes)
            {
                EditorSceneManager.CloseScene(openScene, true);
            }
        }        
    }
}
