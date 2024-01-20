using System.Collections.Generic;
using _Scripts.AssetsProvider;
using _Scripts.Controllers;
using _Scripts.Factory;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Kitchen
{
	public sealed class Customer : MonoBehaviour
	{
		private ICustomerHandler _customerHandler;

		const string ORDERS_PREFABS_PATH = "Prefabs/Orders/{0}";

		private GameObjectFactory _gameObjectFactory;
		
		private List<Order> _orders;
		private float _timer;
		private bool _isActive;

		public Image CustomerImage;
		public List<Sprite> CustomerSprites;
		public Image TimerBar;
		public List<CustomerOrderPlace> OrderPlaces;


		private float _waitTime;

		/// <summary>
		/// Есть ли необслуженные заказы у указанного посетителя.
		/// </summary>
		public bool IsComplete => _orders.Count == 0;

		public void Inititialize(List<Order> orders, ICustomerHandler customerHandler, GameObjectFactory gameObjectFactory)
		{
			_gameObjectFactory = gameObjectFactory;
			_customerHandler = customerHandler;
			_orders = orders;

			if (_orders.Count > OrderPlaces.Count)
			{
				Debug.LogError("There's too many orders for one customer");
				return;
			}

			OrderPlaces.ForEach(x => x.Complete());

			CreateOrders();

			SetRandomSprite();

			_isActive = true;
			_timer = 0f;
		}

		private void CreateOrders()
		{
			var i = 0;
			for (; i < _orders.Count; i++)
			{
				var order = _orders[i];
				var place = OrderPlaces[i];

				_gameObjectFactory.Create<GameEntity>(string.Format(AssetPath.Orders, order.Name), place.transform);
				place.Initialize(order);
			}
		}

		[UsedImplicitly]
		public bool ServeOrder(Order order)
		{
			var place = OrderPlaces.Find(x => x.CurOrder == order);
			if (!place)
			{
				return false;
			}

			_orders.Remove(order);
			place.Complete();
			_timer = Mathf.Max(0f, _timer - 6f);
			return true;
		}

		private void Update()
		{
			if (!_isActive)
				return;

			_timer += Time.deltaTime;

			_waitTime = _customerHandler.GetCustomerTime() - _timer;

			TimerBar.fillAmount = _waitTime / _customerHandler.GetCustomerTime();

			if (_waitTime <= 0f)
			{
				_customerHandler.FreeCustomer(this);
			}
		}

		[ContextMenu("Set random sprite")]
		private void SetRandomSprite()
		{
			CustomerImage.sprite = CustomerSprites[Random.Range(0, CustomerSprites.Count)];
			CustomerImage.SetNativeSize();
		}
	}
}
