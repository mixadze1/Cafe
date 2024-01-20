using System.Collections.Generic;
using _Scripts.GameLogic;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace _Scripts.Kitchen
{
	public sealed class GroupFoodServer : MonoBehaviour
	{
		public List<FoodServer> Servers;
		private CustomersProvider _customerProvider;

		[UsedImplicitly]
		public void TryServe()
		{
			foreach (var server in Servers)
			{
				if (server.TryServeFood())
				{
					return;
				}
			}
		}
	}
}
