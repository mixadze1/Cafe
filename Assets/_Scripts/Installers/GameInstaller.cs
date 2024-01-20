using _Scripts.Controllers;
using _Scripts.Controllers.Customers;
using _Scripts.Controllers.Orders;
using _Scripts.Factory;
using _Scripts.GameLogic;
using UnityEngine;
using Zenject;

namespace _Scripts.Installers
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private Game _game;
        [SerializeField] private CustomersProvider _customersProvider;

        [Header("Factories")] 
        [SerializeField] private CustomerFactory _customerFactopy;
        [SerializeField] private ObjectFactory _objectFactory;
        [SerializeField] private GameObjectFactory _gameObjectFactory;

        public override void InstallBindings()
        {
            BindCustomerProvider();
            BindCustomerController();
            BindGameplayController();
            BindOrdersController();
            
            BindFactoryCustomers();
            BindObjectFactory();
            BindGameObjectFactory();
        }

        private void BindCustomerProvider() => 
            Container.Bind<CustomersProvider>().FromInstance(_customersProvider);

        private void BindOrdersController() =>
            Container.Bind<OrdersController>().AsSingle();

        private void BindCustomerController() => 
            Container.BindInterfacesTo<CustomersController>().AsSingle();

        private void BindGameplayController() =>
            Container.BindInterfacesTo<Game>().FromInstance(_game);

        private void BindGameObjectFactory() => 
            Container.Bind<GameObjectFactory>().FromInstance(_gameObjectFactory);

        private void BindObjectFactory() => 
            Container.Bind<ObjectFactory>().FromInstance(_objectFactory);

        private void BindFactoryCustomers() =>
            Container.Bind<CustomerFactory>().FromInstance(_customerFactopy);
    }
}
