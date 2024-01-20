using _Scripts.AssetsProvider;
using UnityEngine;

namespace _Scripts.Factory
{
    [CreateAssetMenu(menuName = "Factory/Object", fileName = "ObjectFactory")]
    public class ObjectFactory : AssetProvider
    {
        public T Create<T>(string path) where T : Object
        {
            return GetObject<T>(path);
        }
    }
}