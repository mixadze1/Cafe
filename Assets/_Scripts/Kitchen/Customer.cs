using System.Collections.Generic;
using System.Linq;
using _Scripts.AssetsProvider;
using _Scripts.Controllers.Customers;
using _Scripts.Factory;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Kitchen
{
	public sealed class Customer : MonoBehaviour
	{
		private ICustomersController _customersesController;
		private ICustomersInfo _customersInfo;

		private GameObjectFactory _gameObjectFactory;

		private List<Order> _orders;
		private float _timer;
		private bool _isActive;

		private float _waitTime;
		
		public Image CustomerImage;
		public List<Sprite> CustomerSprites;
		public Image TimerBar;
		public List<CustomerOrderPlace> OrderPlaces;
		private ICustomersSettings _customerSettings;

		public bool IsComplete => _orders.Count == 0;

		public void Initialize(List<Order> orders, ICustomersController customersController, ICustomersInfo customersInfo, ICustomersSettings customersSettings, GameObjectFactory gameObjectFactory)
		{
			_customerSettings = customersSettings;
			_customersInfo = customersInfo;
			_customersesController = customersController;
			_gameObjectFactory = gameObjectFactory;
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
				
				_gameObjectFactory.Create<GameEntity>(string.Format(AssetPath.OrdersBurgers, order.Name), place.transform);
				place.Initialize(order);
			}
		}

		[UsedImplicitly]
		public bool ServeOrder(Order order)
		{
			var place = OrderPlaces.Where(x => x.IsActive).ToList().Find(x => x.CurOrder.Name == order.Name);
			if (!place)
			{
				return false;
			}

			_orders.Remove(order);
			place.Complete();
			_timer = Mathf.Max(0f, _timer - _customerSettings.GetReturnTimeAfterGetOrder());
			return true;
		}

		private void Update()
		{
			if (!_isActive)
				return;

			_timer += Time.deltaTime;

			_waitTime = _customersInfo.GetCustomerTime() - _timer;

			TimerBar.fillAmount = _waitTime / _customersInfo.GetCustomerTime();

			if (_waitTime <= 0f)
			{
				_customersesController.FreeCustomer(this);
			}
		}

		public float GetPercentLeftTime() => 
			_waitTime / _customersInfo.GetCustomerTime();

		[ContextMenu("Set random sprite")]
		private void SetRandomSprite()
		{
			CustomerImage.sprite = CustomerSprites[Random.Range(0, CustomerSprites.Count)];
			CustomerImage.SetNativeSize();
		}
	}
}
