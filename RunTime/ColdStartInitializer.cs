using System.Collections;
using UnityEngine;

namespace Games.GrumpyBear.LevelManagement
{
    public class ColdStartInitializer: MonoBehaviour
    {
        [SerializeField] private LocationManager _locationManager;
        [SerializeField] private Location _location;

        private static bool _initialized;

        private IEnumerator Start()
        {
            if (_initialized) yield break;
            yield return _locationManager.Load_CO(_location);
            _initialized = true;
        }

#if UNITY_EDITOR
        private void Reset()
        {
            var go = gameObject;
            go.name = GetType().Name;
            go.tag = "EditorOnly";
        }
        
        [UnityEditor.MenuItem("GameObject/ColdStartInitializer", false, 10)]
        private static void CreateShuttleConfigurationInjector(UnityEditor.MenuCommand menuCommand)
        {
            var go = new GameObject();
            UnityEditor.GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
            go.AddComponent<ColdStartInitializer>();
            UnityEditor.Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            UnityEditor.Selection.activeObject = go;
        }
        #endif
    }
}
