using UnityEngine;

namespace _Scripts.Kitchen {
	public sealed class CustomerOrderPlace : MonoBehaviour
	{
		public Order CurOrder { get; private set; } = null;

		public bool IsActive => CurOrder != null;

		public void Initialize(Order order)
		{
			CurOrder = order;
			gameObject.SetActive(true);
		}

		public void Complete()
		{
			CurOrder = null;
			gameObject.SetActive(false);
		}
	}
}
