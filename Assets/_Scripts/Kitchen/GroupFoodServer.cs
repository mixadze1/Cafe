using System.Collections.Generic;
using _Scripts.GameLogic;
using _Scripts.Providers;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

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
