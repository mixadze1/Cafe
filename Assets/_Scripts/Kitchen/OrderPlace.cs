using System;
using System.Collections.Generic;
using System.Linq;
using _Scripts.Controllers;
using UnityEngine;

namespace _Scripts.Kitchen
{
	public sealed class OrderPlace : AbstractFoodPlace
	{
		private List<Order> _possibleOrders = new();

		public List<FoodPlace> Places = new();
		public event Action CurOrderUpdated;

		[HideInInspector] public List<string> CurOrder = new();

		private void Start()
		{
			_possibleOrders.AddRange(OrdersController.Instance.Orders);
		}

		public override bool TryPlaceFood(Food food)
		{
			if (!CanAddFood(food))
			{
				return false;
			}

			foreach (var place in Places)
			{
				if (!place.TryPlaceFood(food))
				{
					continue;
				}

				CurOrder.Add(food.Name);
				UpdatePossibleOrders();
				CurOrderUpdated?.Invoke();
				return true;
			}

			return false;
		}

		public override void FreePlace()
		{
			_possibleOrders.Clear();
			_possibleOrders.AddRange(OrdersController.Instance.Orders);

			CurOrder.Clear();

			foreach (var place in Places)
			{
				place.FreePlace();
			}

			CurOrderUpdated?.Invoke();
		}

		private bool CanAddFood(Food food)
		{
			if (CurOrder.Contains(food.Name))
			{
				return false;
			}

			foreach (var order in _possibleOrders)
			{
				foreach (var orderFood in order.Foods.Where(x => x.Name == food.Name))
				{
					if (string.IsNullOrEmpty(orderFood.Needs) || CurOrder.Contains(orderFood.Needs))
					{
						return true;
					}
				}
			}

			return false;
		}

		private void UpdatePossibleOrders()
		{
			var ordersToRemove = new List<Order>();
			foreach (var order in _possibleOrders)
			{
				if (order.Foods.Where(x => x.Name == CurOrder[CurOrder.Count - 1]).Count() == 0)
				{
					ordersToRemove.Add(order);
				}
			}

			_possibleOrders.RemoveAll(x => ordersToRemove.Contains(x));
		}
	}
}