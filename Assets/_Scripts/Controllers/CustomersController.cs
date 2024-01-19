using System;
using System.Collections.Generic;
using System.Linq;
using _Scripts.Kitchen;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Scripts.Controllers
{
	public class CustomersController : MonoBehaviour
	{
		private Stack<List<Order>> _orderSets;

		private float _fixedTime;

		private const string CUSTOMER_PREFABS_PATH = "Prefabs/Customer";

		public static CustomersController Instance { get; private set; }

		public int CustomersTargetNumber = 15;
		public float CustomerWaitTime = 18f;
		public float CustomerSpawnTime = 3f;

		public List<CustomerPlace> CustomerPlaces = null;

		public int TotalCustomersGenerated { get; private set; }

		public event Action TotalCustomersGeneratedChanged;

		private bool HasFreePlaces
		{
			get { return CustomerPlaces.Any(x => x.IsFree); }
		}

		public bool IsComplete
		{
			get { return TotalCustomersGenerated >= CustomersTargetNumber && CustomerPlaces.All(x => x.IsFree); }
		}

		private void Awake()
		{
			if (Instance != null)
			{
				Debug.LogError("Another instance of CustomersController already exists!");
			}

			Instance = this;
		}

		private void Start()
		{
			Initialize();
		}

		private void FixedUpdate()
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

		public void Initialize()
		{
			var totalOrders = 0;
			_orderSets = new Stack<List<Order>>();
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

			CustomerPlaces.ForEach(x => x.Free());
			_fixedTime = 0f;

			TotalCustomersGenerated = 0;
			TotalCustomersGeneratedChanged?.Invoke();

			GameplayController.Instance.OrdersTarget = totalOrders - 2;
		}

		/// <summary>
		/// Отпускаем указанного посетителя
		/// </summary>
		/// <param name="customer"></param>
		public void FreeCustomer(Customer customer)
		{
			var place = CustomerPlaces.Find(x => x.CurCustomer == customer);
			if (place == null)
			{
				return;
			}

			place.Free();
			GameplayController.Instance.CheckGameFinish();
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
			var freePlaces = CustomerPlaces.FindAll(x => x.IsFree);
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
			var customerGo = Instantiate(Resources.Load<GameObject>(CUSTOMER_PREFABS_PATH));
			var customer = customerGo.GetComponent<Customer>();

			var orders = _orderSets.Pop();
			customer.Inititialize(orders);

			return customer;
		}

		private Order GenerateRandomOrder()
		{
			var oc = OrdersController.Instance;
			return oc.Orders[Random.Range(0, oc.Orders.Count)];
		}


		private void OnDestroy()
		{
			if (Instance == this)
			{
				Instance = null;
			}
		}
	}
}