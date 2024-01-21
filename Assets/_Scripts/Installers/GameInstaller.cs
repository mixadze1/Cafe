using _Scripts.Controllers;
using _Scripts.Controllers.Customers;
using _Scripts.Controllers.Orders;
using _Scripts.Factory;
using _Scripts.GameLogic;
using _Scripts.Kitchen;
using _Scripts.Providers;
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
            BindCustomersProvider();
            BindCustomersController();
            BindGameplayController();
            BindOrdersController();
            
            BindFactoryCustomers();
            BindObjectFactory();
            BindGameObjectFactory();
            BindPause();
            BindTrashHandler();
        }

        private void BindTrashHandler() => 
            Container.Bind<TrashHandler>().AsSingle();

        private void BindPause() => 
            Container.BindInterfacesTo<Pause.Pause>().AsSingle();

        private void BindCustomersProvider() => 
            Container.Bind<CustomersProvider>().FromInstance(_customersProvider);

        private void BindOrdersController() =>
            Container.Bind<OrdersController>().AsSingle();

        private void BindCustomersController() => 
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
