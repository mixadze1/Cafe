using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace _Scripts.Kitchen
{
	public sealed class GroupFoodServer : MonoBehaviour
	{
		public List<FoodServer> Servers;

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
