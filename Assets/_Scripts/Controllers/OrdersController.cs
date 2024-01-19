using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using _Scripts.Kitchen;
using UnityEngine;

namespace _Scripts.Controllers
{
	public sealed class OrdersController : MonoBehaviour
	{
		private static OrdersController _instance = null;

		private bool _isInit;

		public static OrdersController Instance
		{
			get
			{
				if (!_instance)
					_instance = FindObjectOfType<OrdersController>();

				if (_instance && !_instance._isInit)
				{
					_instance.Initialize();
				}

				return _instance;
			}
			private set => _instance = value;
		}

		public List<Order> Orders = new List<Order>();

		private void Awake()
		{
			if ((_instance != null) && (_instance != this))
			{
				Debug.LogError("Another instance of OrdersController already exists!");
			}

			Instance = this;
		}

		private void Start()
		{
			Initialize();
		}

		private void Initialize()
		{
			if (_isInit)
				return;

			var ordersConfig = Resources.Load<TextAsset>("Configs/Orders");
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

			_isInit = true;
		}

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

		private void OnDestroy()
		{
			if (Instance == this)
			{
				Instance = null;
			}
		}
	}
}
