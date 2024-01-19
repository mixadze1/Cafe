using UnityEngine;

namespace _Scripts.Kitchen
{
	public sealed class CustomerPlace : MonoBehaviour
	{
		public Customer CurCustomer { get; private set; }

		public bool IsFree => CurCustomer == null;

		public void PlaceCustomer(Customer customer)
		{
			CurCustomer = customer;
			customer.transform.SetParent(transform);
			customer.transform.localPosition = Vector3.zero;
		}

		public void Free()
		{
			if (!CurCustomer)
			{
				return;
			}

			var customer = CurCustomer;
			CurCustomer = null;
			Destroy(customer.gameObject);
		}
	}
}
