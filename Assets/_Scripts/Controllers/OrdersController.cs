using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using _Scripts.AssetsProvider;
using _Scripts.Kitchen;
using UnityEditor;
using UnityEngine;
using Zenject;
using ObjectFactory = _Scripts.Factory.ObjectFactory;

namespace _Scripts.Controllers
{
	public sealed class OrdersController : MonoBehaviour
	{
		private ObjectFactory _objectFactory;
		public event Action OnCompleteOrder;

		public List<Order> Orders = new();

		[Inject]
		private void Construct(ObjectFactory objectFactory)
		{
			_objectFactory = objectFactory;
		}

		public void Initialize()
		{
			var ordersConfig = GetOrders();
			var ordersXml = new XmlDocument();
			using (var reader = new StringReader(ordersConfig.ToString()))
			{
				ordersXml.Load(reader);
			}

			var rootElem = ordersXml.DocumentElement;
			foreach (XmlNode node in rootElem.SelectNodes("order"))
			{
				var order = ParseOrder(node);
				Orders.Add(order);
			}

			OnCompleteOrder?.Invoke();
		}

		private TextAsset GetOrders() => 
			_objectFactory.Create<TextAsset>(AssetPath.OrdersConfig);

		public Order FindOrder(List<string> foods)
		{
			return Orders.Find(x =>
			{
				if (x.Foods.Count != foods.Count)
					return false;

				foreach (var food in x.Foods)
				{
					if (x.Foods.Count(f => f.Name == food.Name) != foods.Count(f => f == food.Name))
					{
						return false;
					}
				}

				return true;
			});
		}

		private Order ParseOrder(XmlNode node)
		{
			var foods = new List<Order.OrderFood>();
			foreach (XmlNode foodNode in node.SelectNodes("food"))
			{
				foods.Add(new Order.OrderFood(foodNode.InnerText, foodNode.SelectSingleNode("@needs")?.InnerText));
			}

			return new Order(node.SelectSingleNode("@name").Value, foods);
		}
	}
}
