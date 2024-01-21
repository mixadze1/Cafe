using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace _Scripts.Kitchen
{
	public sealed class Order
	{
		private List<OrderFood> _allFoods;

		public class OrderFood
		{
			public string Name { get; } = null;
			public string Needs { get; } = null;

			public OrderFood(string name, string needs)
			{
				Name = name;
				Needs = needs;
			}
		}

		public readonly string Name;


		public ReadOnlyCollection<OrderFood> Foods
			=> _allFoods.AsReadOnly();

		public Order(string name, List<OrderFood> allFoods)
		{
			Name = name;
			_allFoods = allFoods;
		}
	}
}
