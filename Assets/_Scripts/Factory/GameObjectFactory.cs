using _Scripts.AssetsProvider;
using UnityEngine;

namespace _Scripts.Factory
{
    [CreateAssetMenu(menuName = "Factory/gameObject", fileName = "GameObjectFactory")]
    public class GameObjectFactory : AssetProvider
    {
        public T Create<T>(string path, Transform parent = null) where T : MonoBehaviour => 
            CreateGameObject<T>(path, parent);
    }
}