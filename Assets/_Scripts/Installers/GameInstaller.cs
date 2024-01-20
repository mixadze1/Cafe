using _Scripts.Controllers;
using _Scripts.Factory;
using _Scripts.GameLogic;
using UnityEngine;
using Zenject;

namespace _Scripts.Installers
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private CustomersController _customersController;
        [SerializeField] private Game _game;
        [SerializeField] private OrdersController _ordersController;

        [Header("Factories")] 
        [SerializeField] private CustomerFactory _customerFactopy;
        [SerializeField] private ObjectFactory _objectFactory;
        [SerializeField] private GameObjectFactory _gameObjectFactory;

        public override void InstallBindings()
        {
            BindCustomerController();
            BindGameplayController();
            BindOrdersController();
            
            BindFactoryCustomers();
            BindObjectFactory();
            BindGameObjectFactory();
        }

        private void BindGameObjectFactory() => 
            Container.Bind<GameObjectFactory>().FromInstance(_gameObjectFactory);

        private void BindObjectFactory() => 
            Container.Bind<ObjectFactory>().FromInstance(_objectFactory);

        private void BindFactoryCustomers() =>
            Container.Bind<CustomerFactory>().FromInstance(_customerFactopy);

        private void BindOrdersController() =>
            Container.Bind<OrdersController>().FromInstance(_ordersController);

        private void BindGameplayController() =>
            Container.BindInterfacesTo<Game>().FromInstance(_game);

        private void BindCustomerController() =>
            Container.Bind<CustomersController>().FromInstance(_customersController);
    }
}
