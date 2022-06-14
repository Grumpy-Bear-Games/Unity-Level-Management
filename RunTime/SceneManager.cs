using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Games.GrumpyBear.LevelManagement
{
    [CreateAssetMenu(menuName = "Grumpy Bear Games/Level Management/Scene Manager")]
    public class SceneManager: ScriptableObject
    {
        public static event Action<SceneGroup> OnLocationChanged;  
            
        [SerializeField] private List<SceneReference> _globalScenes = new();
        [SerializeField] private List<SceneGroup> _sceneGroups = new();
        public IReadOnlyList<SceneReference> GlobalScenes => _globalScenes;
        public IReadOnlyList<SceneGroup> SceneGroups => _sceneGroups;

        public void Load(SceneGroup sceneGroup) => SceneManagerHelper.Instance.StartCoroutine(Load_CO(sceneGroup));

        public IEnumerator Load_CO(SceneGroup sceneGroup)
        {
            var sceneReferencesToLoad = sceneGroup.Scenes.Concat(_globalScenes).Select(scene => scene.ScenePath);
            yield return SceneLoader.LoadExactlyByScenePath(sceneReferencesToLoad, sceneGroup.ActiveScene.ScenePath);
            OnLocationChanged?.Invoke(sceneGroup);
        }
        
        #if UNITY_EDITOR
        public const string GlobalScenesPropertyName = nameof(_globalScenes);
        public const string SceneGroupsPropertyName = nameof(_sceneGroups);
        #endif
    }
}
