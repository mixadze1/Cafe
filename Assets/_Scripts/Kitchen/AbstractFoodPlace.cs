using UnityEngine;

namespace _Scripts.Kitchen
{
	public abstract class AbstractFoodPlace : MonoBehaviour
	{
		public abstract bool TryPlaceFood(Food food);
		public abstract void FreePlace();
	}
}
