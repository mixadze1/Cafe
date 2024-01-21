using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace _Scripts.Kitchen {
	[RequireComponent(typeof(FoodPlace))]
	public sealed class FoodTransfer : MonoBehaviour
	{
		public List<AbstractFoodPlace> DestPlaces = new();

		FoodPlace _place = null;

		private void Awake()
		{
			_place = GetComponent<FoodPlace>();
		}

		[UsedImplicitly]
		public void TryTransferFood()
		{
			var food = _place.CurFood;

			if (food == null)
				return;
			
			if (food.CurStatus != Food.FoodStatus.Cooked)
			{
				_place.TryPlaceFood(food);
				return;
			}

			foreach (var place in DestPlaces)
			{
				if (!place.TryPlaceFood(food))
				{
					continue;
				}

				_place.FreePlace();
				return;
			}
		}
	}
}
