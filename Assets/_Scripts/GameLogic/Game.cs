using System;
using _Scripts.Controllers;
using _Scripts.Kitchen;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using Zenject;

namespace _Scripts.GameLogic
{
	public sealed class Game : MonoBehaviour, IGameHandler, IGameInfo, IGameChanges
	{
		private CustomersController _customerController;
		private OrdersController _orderController;
		
		public event Action OnWinGame;
		public event Action OnLoseGame;
		public event Action OnEndGame;
		public event Action OnRestartGame;
		private int _ordersTarget = 0;

		public event Action TotalOrdersServedChanged;

		public int TotalOrdersServed { get; private set; } = 0;

		public int OrdersTarget
		{
			get => _ordersTarget;
			set
			{
				_ordersTarget = value;
				TotalOrdersServedChanged?.Invoke();
			}
		}

		[Inject]
		private void Construct(CustomersController customersController, OrdersController ordersController)
		{
			_customerController = customersController;
			_orderController = ordersController;
		}

		private void Start()
		{
			_orderController.Initialize();
			_customerController.Initialize();
			Initialize();
		}
		
		private void Initialize()
		{
			TotalOrdersServed = 0;
			Time.timeScale = 1f;
			TotalOrdersServedChanged?.Invoke();
			OrdersTarget = _customerController.GetTargetOrders();
		}

		public int GetOrdersTarget() => 
			OrdersTarget;


		public int GetTotalOrdersServed() => 
			TotalOrdersServed;

		public void CheckGameFinish()
		{
			if (_customerController.IsComplete())
			{
				EndGame(TotalOrdersServed >= OrdersTarget);
			}
		}

		private void EndGame(bool win)
		{
			Time.timeScale = 0f;
			OnEndGame?.Invoke();
			if (win)
			{
				OnWinGame?.Invoke();
			}
			else
			{
				OnLoseGame?.Invoke();
			}
		}
		

		[UsedImplicitly]
		public bool TryServeOrder(Order order)
		{
			if (!_customerController.ServeOrder(order))
			{
				return false;
			}

			TotalOrdersServed++;
			TotalOrdersServedChanged?.Invoke();
			CheckGameFinish();
			return true;
		}

		public void Restart()
		{
			Initialize();
			OnRestartGame?.Invoke();

			foreach (var place in FindObjectsOfType<AbstractFoodPlace>())
			{
				place.FreePlace();
			}
		}

		public void CloseGame()
		{
#if UNITY_EDITOR
			EditorApplication.isPlaying = false;
#else
			Application.Quit();
#endif
		}
	}

	public interface IGameChanges
	{
		event Action TotalOrdersServedChanged;
	}
}
