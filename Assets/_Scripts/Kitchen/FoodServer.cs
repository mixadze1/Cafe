using System.Collections.Generic;
using _Scripts.Controllers;
using UnityEngine;

namespace _Scripts.Kitchen {
	[RequireComponent(typeof(FoodPlace))]
	public sealed class FoodServer : MonoBehaviour {

		private FoodPlace _place;

		private void Start()
		{
			_place = GetComponent<FoodPlace>();
		}

		public bool TryServeFood()
		{
			if (_place.IsFree || (_place.CurFood.CurStatus != Food.FoodStatus.Cooked))
			{
				return false;
			}

			var order = OrdersController.Instance.FindOrder(new List<string>(1) { _place.CurFood.Name });
			if ((order == null) || !GameplayController.Instance.TryServeOrder(order))
			{
				return false;
			}

			_place.FreePlace();
			return true;
		}
	}
}
