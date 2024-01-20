using System;
using System.Collections.Generic;
using System.Linq;
using _Scripts.AssetsProvider;
using _Scripts.Controllers.Orders;
using _Scripts.Factory;
using _Scripts.GameLogic;
using _Scripts.Kitchen;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace _Scripts.Controllers.Customers
{
	public class CustomersController : ICustomerHandler, IDisposable, IFixedTickable, ICustomersInfo
	{
		private float _customerWaitTime = 18f;

		private Stack<List<Order>> _orderSets;
		private CustomerFactory _customerFactory;

		private IGameHandler _gameHandler;

		private float _fixedTime;

		public int CustomersTargetNumber;
		public float CustomerSpawnTime;

		private List<CustomerPlace> _customerPlaces = new();
		private int _totalOrderTargetCondition;
		private OrdersController _ordersController;
		private GameObjectFactory _gameObjectFactory;
		private CustomersProvider _customerProvider;

		public int TotalCustomersGenerated { get; private set; }

		public event Action TotalCustomersGeneratedChanged;

		public int GetTotalCustomersGenerated()
		{
			return TotalCustomersGenerated;
		}

		public int GetCustomersTargetNumber()
		{
			return CustomersTargetNumber;
		}

		private bool HasFreePlaces =>
			_customerPlaces.Any(x => x.IsFree);


		[Inject]
		private void Construct(CustomerFactory customerFactory, IGameHandler gameHandler,
			OrdersController ordersController, GameObjectFactory gameObjectFactory, CustomersProvider customersProvider)
		{
			_customerProvider = customersProvider;
			_gameObjectFactory = gameObjectFactory;
			_ordersController = ordersController;
			_gameHandler = gameHandler;
			_customerFactory = customerFactory;
			_gameHandler.OnRestartGame += OnRestartGame;

			_customerPlaces.AddRange(customersProvider.CustomerPlaces);
			CustomersTargetNumber = _customerProvider.CustomersTargetNumber;
			CustomerSpawnTime = _customerProvider.CustomerSpawnTime;
		}

		public void Initialize()
		{
			var totalOrders = 0;
			_orderSets = new Stack<List<Order>>();

			totalOrders = GenerateOrders(totalOrders);

			_customerPlaces.ForEach(x => x.Free());
			_fixedTime = 0f;
			TotalCustomersGenerated = 0;

			TotalCustomersGeneratedChanged?.Invoke();

			_totalOrderTargetCondition = totalOrders - 2;
		}

		public int GetTargetOrders() =>
			_totalOrderTargetCondition;

		public bool IsComplete() =>
			TotalCustomersGenerated >= CustomersTargetNumber && _customerPlaces.All(x => x.IsFree);

		public void FixedTick()
		{
			if (!HasFreePlaces)
			{
				return;
			}

			_fixedTime += Time.fixedDeltaTime;

			if (TotalCustomersGenerated >= CustomersTargetNumber || !(_fixedTime > CustomerSpawnTime))
			{
				return;
			}

			SpawnCustomer();
			_fixedTime = 0f;
		}

		private void OnRestartGame() =>
			Initialize();

		private int GenerateOrders(int totalOrders)
		{
			for (var i = 0; i < CustomersTargetNumber; i++)
			{
				var orders = new List<Order>();
				var ordersNum = Random.Range(1, 4);
				for (var j = 0; j < ordersNum; j++)
				{
					orders.Add(GenerateRandomOrder());
				}

				_orderSets.Push(orders);
				totalOrders += ordersNum;
			}

			return totalOrders;
		}

		/// <summary>
		/// Отпускаем указанного посетителя
		/// </summary>
		/// <param name="customer"></param>
		public void FreeCustomer(Customer customer)
		{
			var place = _customerPlaces.Find(x => x.CurCustomer == customer);
			if (place == null)
			{
				return;
			}

			place.Free();
			_gameHandler.CheckGameFinish();
		}

		/// <summary>
		///  Пытаемся обслужить посетителя с заданным заказом и наименьшим оставшимся временем ожидания.
		///  Если у посетителя это последний оставшийся заказ из списка, то отпускаем его.
		/// </summary>
		/// <param name="order">Заказ, который пытаемся отдать</param>
		/// <returns>Флаг - результат, удалось ли успешно отдать заказ</returns>
		public bool ServeOrder(Order order)
		{
			throw new NotImplementedException("ServeOrder: this feature is not implemented.");
		}

		private void SpawnCustomer()
		{
			var freePlaces = _customerPlaces.FindAll(x => x.IsFree);
			if (freePlaces.Count <= 0)
			{
				return;
			}

			var place = freePlaces[Random.Range(0, freePlaces.Count)];
			place.PlaceCustomer(GenerateCustomer());
			TotalCustomersGenerated++;
			TotalCustomersGeneratedChanged?.Invoke();
		}

		private Customer GenerateCustomer()
		{
			var customer = _customerFactory.CreateCustomer(AssetPath.CustomerPrefab);

			var orders = _orderSets.Pop();
			customer.Initialize(orders, customerHandler: this, customersInfo: this, _gameObjectFactory);

			return customer;
		}

		private Order GenerateRandomOrder() =>
			_ordersController.Orders[Random.Range(0, _ordersController.Orders.Count)];

		public float GetCustomerTime()
		{
			if (_customerWaitTime < 0)
				Debug.LogError($"Set current wait customer {_customerWaitTime}.");

			return _customerWaitTime;
		}

		public void Dispose()
		{
			if (_gameHandler != null)
				_gameHandler.OnRestartGame -= OnRestartGame;
		}
	}
}