using UnityEditor;
using UnityEngine;

namespace Games.GrumpyBear.LevelManagement.Editor
{
    
    [CustomEditor(typeof(LocationColdStartInitializer))]
    public class LocationColdStartInitializerEditor: UnityEditor.Editor
    {
        private SerializedProperty _locationProperty;

        private void OnEnable()
        {
            _locationProperty = serializedObject.FindProperty("_sceneGroup");
        }

        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            var sceneGroup = _locationProperty.objectReferenceValue as SceneGroup;
            
            if (sceneGroup == null) EditorGUILayout.HelpBox("Location missing", MessageType.Warning);
            
            var canLoad = (sceneGroup != null) && (sceneGroup.Scenes.Count > 0);  
            GUI.enabled = canLoad;
            if (GUILayout.Button("Load location")) sceneGroup.LoadInEditor();
            GUI.enabled = true;
        }
    }
}
