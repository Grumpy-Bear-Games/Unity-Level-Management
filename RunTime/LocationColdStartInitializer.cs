using System.Collections;
using UnityEngine;

namespace Games.GrumpyBear.LevelManagement
{
    public class LocationColdStartInitializer: MonoBehaviour
    {
        private const string DEFAULT_NAME = "[Location Coldstart Initializer]";
        
        [SerializeField] private SceneGroup _sceneGroup;

        private static bool _initialized;

        private IEnumerator Start()
        {
            if (_initialized) yield break;
            yield return _sceneGroup.Load_CO();
            _initialized = true;
        }

        #if UNITY_EDITOR
        private void Reset()
        {
            var go = gameObject;
            go.name = DEFAULT_NAME;
            go.tag = "EditorOnly";
        }
        
        [UnityEditor.MenuItem("GameObject/Grumpy Bear Games/Level Management/Location ColdStart Initializer", false, 10)]
        private static void CreateLocationColdStartInitializer(UnityEditor.MenuCommand menuCommand)
        {
            var go = new GameObject(DEFAULT_NAME, typeof(LocationColdStartInitializer));
            UnityEditor.GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
            UnityEditor.Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            UnityEditor.Selection.activeObject = go;
        }
        #endif
    }
}
