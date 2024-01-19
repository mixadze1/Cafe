using UnityEngine;

namespace _Scripts.Kitchen
{
	public sealed class FoodPresenter : MonoBehaviour
	{
		public string FoodName = string.Empty;

		public FoodVisualizersSet Set;

		public FoodPlace Place;
		
		private void Start()
		{
			Set?.Hide();
			if (Place)
			{
				Place.FoodPlaceUpdated += OnFoodPlaceUpdated;
			}
		}

		private void OnFoodPlaceUpdated()
		{
			if (Place.IsFree)
			{
				Set?.ShowEmpty();
			}
			else
			{
				if (Place.CurFood.Name == FoodName)
				{
					Set?.ShowStatus(Place.CurFood.CurStatus);
				}
				else
				{
					Set?.Hide();
				}
			}
		}

		private void OnDestroy()
		{
			if (Place)
			{
				Place.FoodPlaceUpdated -= OnFoodPlaceUpdated;
			}
		}
	}
}
