using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Kitchen
{
	public sealed class OrderVisualizer : MonoBehaviour
	{
		public List<FoodVisualizer> Visualizers = new();

		private void Awake()
		{
			Clear();
		}

		private void Clear()
		{
			Visualizers.ForEach(x => x.SetEnabled(false));
		}

		public void Init(List<string> foods)
		{
			Clear();
			foreach (var vis in Visualizers)
			{
				if (foods.Contains(vis.Name))
				{
					vis.SetEnabled(true);
				}
			}
		}
	}
}
