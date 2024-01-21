using System;
using _Scripts.Controllers;
using _Scripts.Controllers.Customers;
using _Scripts.Controllers.Orders;
using _Scripts.Kitchen;
using _Scripts.Pause;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using Zenject;

namespace _Scripts.GameLogic
{
	public sealed class Game : MonoBehaviour, IGameHandler, IGameInfo, IGameChanges, IGameDelegates
	{
		private ICustomersInfo _customersInfo;
		private ICustomersController _customersController;
		private IPauseProvider _pauseProvider;
		private OrdersController _orderController;

		private int _ordersTarget;
		private int _totalOrdersServed;

		private int OrdersTarget
		{
			get => _ordersTarget;
			set
			{
				_ordersTarget = value;
				TotalOrdersServedChanged?.Invoke();
			}
		}
		public event Action TotalOrdersServedChanged;
		public event Action OnWinGame;
		public event Action OnLoseGame;
		public event Action OnEndGame;
		public event Action OnStartGame;
		public event Action OnRestartGame;

		[Inject]
		private void Construct(IPauseProvider pauseProvider, ICustomersInfo customersInfo, ICustomersController customersController, OrdersController ordersController)
		{
			_pauseProvider = pauseProvider;
			_customersController = customersController;
			_customersInfo = customersInfo;
			_orderController = ordersController;
		}

		private void Start()
		{
			_orderController.Initialize();
			_customersController.Initialize();
			Initialize();
			OnStartGame?.Invoke();
		}

		public int GetOrdersTarget() => 
			OrdersTarget;
		
		public int GetTotalOrdersServed() => 
			_totalOrdersServed;

		public void CheckGameFinish()
		{
			if (_customersController.IsComplete())
			{
				EndGame(_totalOrdersServed >= OrdersTarget);
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

		private void Initialize()
		{
			_totalOrdersServed = 0;
			_pauseProvider.UnPauseGame();
			TotalOrdersServedChanged?.Invoke();
			OrdersTarget = _customersInfo.GetTargetOrders();
		}
		
		public bool TryServeOrder(Order order)
		{
			if (!_customersController.ServeOrder(order))
			{
				return false;
			}

			_totalOrdersServed++;
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
}
