using System;
using JetBrains.Annotations;
using UnityEngine;

namespace _Scripts.Kitchen
{
	[RequireComponent(typeof(FoodPlace))]
	public sealed class FoodTrasher : MonoBehaviour
	{
		private FoodPlace _place;
		private float _timer;

		void Start()
		{
			_place = GetComponent<FoodPlace>();
			_timer = Time.realtimeSinceStartup;
		}

		/// <summary>
		/// Освобождает место по двойному тапу если еда на этом месте сгоревшая.
		/// </summary>
		[UsedImplicitly]
		public void TryTrashFood()
		{
			throw new NotImplementedException("TryTrashFood: this feature is not implemented");
		}

	}
}
