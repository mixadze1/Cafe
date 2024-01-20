using System.Collections.Generic;
using _Scripts.Controllers;
using _Scripts.GameLogic;
using UnityEngine;
using Zenject;

namespace _Scripts.Kitchen {
	[RequireComponent(typeof(FoodPlace))]
	public sealed class FoodServer : MonoBehaviour {

		private FoodPlace _place;
		private IGameInfo _gameInfo;
		private OrdersController _ordersController;

		[Inject]
		private void Construct(IGameInfo gameInfo,OrdersController ordersController)
		{
			_ordersController = ordersController;
			_gameInfo = gameInfo;
			_place = GetComponent<FoodPlace>();
		}

		public bool TryServeFood()
		{
			if (_place.IsFree || (_place.CurFood.CurStatus != Food.FoodStatus.Cooked))
				return false;

			var order = _ordersController.FindOrder(new List<string>(1) { _place.CurFood.Name });

			if ((order == null) || !_gameInfo.TryServeOrder(order))
				return false;

			_place.FreePlace();
			return true;
		}
	}
}
