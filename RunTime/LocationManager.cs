using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Games.GrumpyBear.LevelManagement
{
    [CreateAssetMenu(menuName = "Grumpy Bear Games/Level Management/Location Manager")]
    public class LocationManager: ScriptableObject
    {
        public static event Action<Location> OnLocationChanged;  
            
        [SerializeField] private List<SceneReference> _globalScenes = new List<SceneReference>();

        public IReadOnlyList<SceneReference> GlobalScenes => _globalScenes;

        public void Load(Location location) => LocationManagerHelper.Instance.StartCoroutine(Load_CO(location));

        public IEnumerator Load_CO(Location location)
        {
            var sceneReferencesToLoad = location.Scenes.Concat(_globalScenes).Select(scene => scene.ScenePath);
            yield return SceneLoader.LoadExactlyByScenePath(sceneReferencesToLoad, location.ActiveScene.ScenePath);
            OnLocationChanged?.Invoke(location);
        }
    }
}
