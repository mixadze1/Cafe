using _Scripts.Controllers.Orders;
using _Scripts.GameLogic;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace _Scripts.Kitchen
{
	[RequireComponent(typeof(OrderPlace))]
	public sealed class OrderServer : MonoBehaviour
	{
		private OrderPlace _orderPlace;
		private IGameInfo _gameInfo;
		private OrdersController _ordersController;

		[Inject]
		private void Construct(IGameInfo gameInfo, OrdersController ordersController)
		{
			_ordersController = ordersController;
			_gameInfo = gameInfo;
			_orderPlace = GetComponent<OrderPlace>();
		}

		[UsedImplicitly]
		public void TryServeOrder()
		{
			var order = _ordersController.FindOrder(_orderPlace.CurOrder);
			
			if ((order == null) || !_gameInfo.TryServeOrder(order))
			{
				return;
			}

			_orderPlace.FreePlace();
		}
	}
}