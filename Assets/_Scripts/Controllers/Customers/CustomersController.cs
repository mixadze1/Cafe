using System;
using System.Collections.Generic;
using System.Linq;
using _Scripts.AssetsProvider;
using _Scripts.Controllers.Orders;
using _Scripts.Factory;
using _Scripts.GameLogic;
using _Scripts.Kitchen;
using _Scripts.Providers;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace _Scripts.Controllers.Customers
{
	public sealed class CustomersController : ICustomersController, IDisposable, IFixedTickable, ICustomersInfo, ICustomersSettings
	{
		private List<CustomerPlace> _customerPlaces = new();
		private Stack<List<Order>> _orderSets;
		private CustomerFactory _customerFactory;

		private IGameHandler _gameHandler;
		private IGameDelegates _gameDelegates;

		private OrdersController _ordersController;
		private GameObjectFactory _gameObjectFactory;

		private float _fixedTime;
		private int _totalCustomersGenerated;
		private int _totalOrderTargetCondition;
		private int _returnTimeAfterGetOrder;

		[Header("Customer settings:")]
		private float _customerWaitTime;
		private float _customerSpawnTime;
		private int _customersTargetNumber;
		private int _decreaseAmountOrders;

		private bool HasFreePlaces =>
			_customerPlaces.Any(x => x.IsFree);

		public event Action TotalCustomersGeneratedChanged;

		[Inject]
		private void Construct(CustomerFactory customerFactory, IGameHandler gameHandler, IGameDelegates gameDelegates,
			OrdersController ordersController, GameObjectFactory gameObjectFactory, CustomersProvider customersProvider)
		{
			_gameDelegates = gameDelegates;
			_gameObjectFactory = gameObjectFactory;
			_ordersController = ordersController;
			_gameHandler = gameHandler;
			_customerFactory = customerFactory;
			_gameDelegates.OnRestartGame += OnRestartGame;

			SetCustomers(customersProvider);
		}

		private void SetCustomers(CustomersProvider customersProvider)
		{
			_customerPlaces.AddRange(customersProvider.CustomerPlaces);
			_customersTargetNumber = customersProvider.CustomersTargetNumber;
			_customerSpawnTime = customersProvider.CustomerSpawnTime;
			_customerWaitTime = customersProvider.CustomerWaitTime;
			_decreaseAmountOrders = customersProvider.DecreaseAmountOrdersFromMaximum;
			_returnTimeAfterGetOrder = customersProvider.ReturnTimeAfterGetOrder;
		}

		public void Initialize()
		{
			var totalOrders = 0;
			_orderSets = new Stack<List<Order>>();

			totalOrders = GenerateOrders(totalOrders);

			_customerPlaces.ForEach(x => x.Free());
			_fixedTime = 0f;
			_totalCustomersGenerated = 0;

			TotalCustomersGeneratedChanged?.Invoke();
			
			_totalOrderTargetCondition = totalOrders - _decreaseAmountOrders;
		}

		public int GetTotalCustomersGenerated() => 
			_totalCustomersGenerated;

		public int GetCustomersTargetNumber()
		{
			if (_customersTargetNumber <= 0) 
				Debug.LogError($"Not correct value customers {_customersTargetNumber}");

			return _customersTargetNumber;
		}

		public int GetTargetOrders()
		{
			if (_totalOrderTargetCondition <= 0)
				Debug.LogError($"Not correct value orders {_totalOrderTargetCondition}");
			
			return _totalOrderTargetCondition;
		}

		public bool IsComplete() =>
			_totalCustomersGenerated >= _customersTargetNumber && _customerPlaces.All(x => x.IsFree);

		public void FixedTick()
		{
			if (!HasFreePlaces)
			{
				return;
			}

			_fixedTime += Time.fixedDeltaTime;

			if (_totalCustomersGenerated >= _customersTargetNumber || !(_fixedTime > _customerSpawnTime))
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
			for (var i = 0; i < _customersTargetNumber; i++)
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
		
		public void FreeCustomer(Customer customer)
		{
			var place = _customerPlaces.Find(x => x.CurCustomer == customer);
			if (place == null)
				return;

			place.Free();
			_gameHandler.CheckGameFinish();
		}
		
		public bool ServeOrder(Order order)
		{
			IEnumerable<CustomerPlace> currentCustomers = _customerPlaces.Where(x => !x.IsFree).OrderBy(x => x.GetLeftTime());
			foreach (var c in currentCustomers)
			{
				if (c.CurCustomer.ServeOrder(order))
				{
					if(c.CurCustomer.IsComplete)
						c.Free();
					
					return true;
				}
			}
			return false;
		}

		private void SpawnCustomer()
		{
			var freePlaces = _customerPlaces.FindAll(x => x.IsFree);
			
			if (freePlaces.Count <= 0)
				return;

			var place = freePlaces[Random.Range(0, freePlaces.Count)];
			place.PlaceCustomer(GenerateCustomer());
			_totalCustomersGenerated++;
			TotalCustomersGeneratedChanged?.Invoke();
		}

		private Customer GenerateCustomer()
		{
			var customer = _customerFactory.CreateCustomer(AssetPath.CustomerPrefab);

			var orders = _orderSets.Pop();
			customer.Initialize(orders, customersController: this, customersInfo: this, customersSettings:this, _gameObjectFactory);

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

		public int GetReturnTimeAfterGetOrder() => 
			_returnTimeAfterGetOrder;

		public void Dispose()
		{
			if (_gameDelegates != null)
				_gameDelegates.OnRestartGame -= OnRestartGame;
		}
	}
}