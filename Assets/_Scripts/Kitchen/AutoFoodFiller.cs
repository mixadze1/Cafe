using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Kitchen
{
	public sealed class AutoFoodFiller : MonoBehaviour
	{
		public string FoodName = null;
		public List<AbstractFoodPlace> Places = new();

		private void Update()
		{
			foreach (var place in Places)
			{
				place.TryPlaceFood(new Food(FoodName));
			}
		}
	}
}
