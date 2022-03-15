using System.Collections.Generic;
using UnityEngine;

namespace Games.GrumpyBear.LevelManagement
{
    [CreateAssetMenu(menuName = "Grumpy Bear Games/Level Management/Location")]
    public class Location : ScriptableObject
    {
        [SerializeField] private List<SceneReference> _scenes = new List<SceneReference>();
        public IReadOnlyList<SceneReference> Scenes => _scenes;
        public SceneReference ActiveScene => _scenes[0];
    }
}
