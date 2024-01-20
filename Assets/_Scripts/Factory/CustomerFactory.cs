using _Scripts.AssetsProvider;
using _Scripts.Kitchen;
using UnityEngine;

namespace _Scripts.Factory
{
    [CreateAssetMenu(menuName = "Factory/Customer", fileName = "CustomerFactory")]
    public class CustomerFactory : AssetProvider
    {

        public Customer CreateCustomer(string path, Transform parent = null)
        {
            return CreateGameObject<Customer>(path, parent);
        }
    }
}
