using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Games.GrumpyBear.LevelManagement.Editor
{
    
    [CustomEditor(typeof(LocationColdStartInitializer))]
    public class LocationColdStartInitializerEditor: UnityEditor.Editor
    {
        private SerializedProperty _locationManagerProperty;
        private SerializedProperty _locationProperty;

        private void OnEnable()
        {
            _locationManagerProperty = serializedObject.FindProperty("_locationManager");
            _locationProperty = serializedObject.FindProperty("_location");
        }

        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            var locationManager = _locationManagerProperty.objectReferenceValue as LocationManager;
            var location = _locationProperty.objectReferenceValue as Location;
            
            if (locationManager == null) EditorGUILayout.HelpBox("Location Manager missing", MessageType.Warning);
            if (location == null) EditorGUILayout.HelpBox("Location missing", MessageType.Warning);
            
            var canLoad = (locationManager != null) && (location != null) &&
                          ((locationManager.GlobalScenes.Count > 0) || (location.Scenes.Count > 0));  
            GUI.enabled = canLoad;
            if (GUILayout.Button("Load location")) LoadScene(locationManager, location);
            GUI.enabled = true;
        }

        private void LoadScene(LocationManager locationManager, Location location)
        {
            var openScenes = Enumerable.Range(0, EditorSceneManager.loadedSceneCount)
                .Select(EditorSceneManager.GetSceneAt)
                .Where(scene => location.Scenes.All(x => scene.path != x.ScenePath))
                .Where(scene => locationManager.GlobalScenes.All(x => scene.path != x.ScenePath))
                .ToArray();
            if (!EditorSceneManager.SaveModifiedScenesIfUserWantsTo(openScenes)) return;
            foreach (var sceneAsset in location.Scenes)
            {
                EditorSceneManager.OpenScene(sceneAsset.ScenePath, OpenSceneMode.Additive);
            }
            foreach (var sceneAsset in locationManager.GlobalScenes)
            {
                EditorSceneManager.OpenScene(sceneAsset.ScenePath, OpenSceneMode.Additive);
            }
            EditorSceneManager.SetActiveScene(EditorSceneManager.GetSceneByPath(location.ActiveScene.ScenePath));
            foreach (var openScene in openScenes)
            {
                EditorSceneManager.CloseScene(openScene, true);
            }
        }
    }
}
