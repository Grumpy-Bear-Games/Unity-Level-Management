using UnityEngine;

namespace Games.GrumpyBear.LevelManagement
{
    internal class LocationManagerHelper : MonoBehaviour
    {
        private static LocationManagerHelper _instance;
        public static LocationManagerHelper Instance {
            get
            {
                if (_instance == null)
                {
                    var go = new GameObject("[LocationManager Helper]", typeof(LocationManagerHelper))
                    {
                        hideFlags = HideFlags.DontSave | HideFlags.NotEditable | HideFlags.HideAndDontSave | HideFlags.HideInHierarchy
                    };
                    DontDestroyOnLoad(go);
                    _instance = go.GetComponent<LocationManagerHelper>();
                }

                return _instance;
            }
        }
    }
}
