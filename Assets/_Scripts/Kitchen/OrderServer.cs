using _Scripts.Controllers;
using JetBrains.Annotations;
using UnityEngine;

namespace _Scripts.Kitchen
{
	[RequireComponent(typeof(OrderPlace))]
	public sealed class OrderServer : MonoBehaviour
	{
		private OrderPlace _orderPlace;

		private void Start()
		{
			_orderPlace = GetComponent<OrderPlace>();
		}

		[UsedImplicitly]
		public void TryServeOrder()
		{
			var order = OrdersController.Instance.FindOrder(_orderPlace.CurOrder);
			if ((order == null) || !GameplayController.Instance.TryServeOrder(order))
			{
				return;
			}

			_orderPlace.FreePlace();
		}
	}
}