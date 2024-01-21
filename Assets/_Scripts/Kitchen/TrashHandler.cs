using System;

namespace _Scripts.Kitchen
{
    public class TrashHandler
    {
        public event Action OnThrowTrash;
		
        public void ThrowTrash(FoodPlace foodPlace)
        {
            foodPlace.FreePlace();
            OnThrowTrash?.Invoke();
        }
    }
}