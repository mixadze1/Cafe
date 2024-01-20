using UnityEngine;

namespace _Scripts.AssetsProvider
{
    public class AssetProvider : ScriptableObject
    {
        protected static T CreateGameObject<T>(string path, Transform parent) where T : MonoBehaviour
        {
            var prefab = Resources.Load<T>(path);
            return Instantiate(prefab, parent);
        }

        protected static T GetObject<T>(string path) where T : Object
        {
            var instance = Resources.Load<T>(path);
            return instance;
        }
    }
}