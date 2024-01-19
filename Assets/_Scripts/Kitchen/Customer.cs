using System.Collections.Generic;
using _Scripts.Controllers;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Kitchen {
	public sealed class Customer : MonoBehaviour
	{
		public Image CustomerImage = null;
		public List<Sprite> CustomerSprites = null;
		public Image TimerBar = null;
		public List<CustomerOrderPlace> OrderPlaces = null;

		const string ORDERS_PREFABS_PATH = "Prefabs/Orders/{0}";

		private List<Order> _orders = null;
		private float _timer = 0f;
		private bool _isActive = false;

		private float _waitTime => CustomersController.Instance.CustomerWaitTime - _timer;

		/// <summary>
		/// Есть ли необслуженные заказы у указанного посетителя.
		/// </summary>
		public bool IsComplete => _orders.Count == 0;

		public void Inititialize(List<Order> orders)
		{
			_orders = orders;

			if (_orders.Count > OrderPlaces.Count)
			{
				Debug.LogError("There's too many orders for one customer");
				return;
			}

			OrderPlaces.ForEach(x => x.Complete());

			var i = 0;
			for (; i < _orders.Count; i++)
			{
				var order = _orders[i];
				var place = OrderPlaces[i];
				Instantiate(Resources.Load<GameObject>(string.Format(ORDERS_PREFABS_PATH, order.Name)), place.transform,
					false);
				place.Initialize(order);
			}

			SetRandomSprite();

			_isActive = true;
			_timer = 0f;
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
			TimerBar.fillAmount = _waitTime / CustomersController.Instance.CustomerWaitTime;

			if (_waitTime <= 0f)
			{
				CustomersController.Instance.FreeCustomer(this);
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
