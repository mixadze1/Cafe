using System;
using UnityEngine;

namespace _Scripts.Kitchen
{
	public class FoodPlace : AbstractFoodPlace
	{
		private float _timer;
		
		public bool Cook;
		public float CookTime;
		public float OvercookTime;

		public event Action FoodPlaceUpdated;

		public Food CurFood { get; private set; }
		public bool IsCooking { get; private set; }

		public bool IsFree => CurFood == null;

		public float TimerNormalized
		{
			get
			{
				if (IsFree || !Cook || !IsCooking)
				{
					return 0f;
				}

				if (CurFood.CurStatus == Food.FoodStatus.Raw)
				{
					return _timer / CookTime;
				}

				return _timer / OvercookTime;
			}
		}

		private void Update()
		{
			if (IsFree || !Cook || !IsCooking)
				return;

			_timer += Time.deltaTime;
			switch (CurFood.CurStatus)
			{
				case Food.FoodStatus.Raw:
				{
					if (_timer > CookTime)
					{
						CurFood.CookStep();
						_timer = 0f;
						if (OvercookTime <= 0f)
						{
							IsCooking = false;
						}

						FoodPlaceUpdated?.Invoke();
					}

					break;
				}
				case Food.FoodStatus.Cooked:
				{
					if (_timer > OvercookTime)
					{
						CurFood.CookStep();
						_timer = 0f;
						IsCooking = false;
						FoodPlaceUpdated?.Invoke();
					}

					break;
				}
			}
		}

		public override bool TryPlaceFood(Food food)
		{
			if (!IsFree)
			{
				return false;
			}

			CurFood = food;
			if (Cook && CurFood.CurStatus != (Food.FoodStatus.Overcooked))
			{
				IsCooking = true;
			}

			FoodPlaceUpdated?.Invoke();
			return true;
		}

		public override void FreePlace()
		{
			CurFood = null;
			_timer = 0f;
			IsCooking = false;
			FoodPlaceUpdated?.Invoke();
		}
	}
}
