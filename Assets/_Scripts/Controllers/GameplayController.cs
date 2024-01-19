using System;
using _Scripts.Kitchen;
using _Scripts.UI;
using JetBrains.Annotations;
using UnityEngine;

namespace _Scripts.Controllers
{
	public sealed class GameplayController : MonoBehaviour
	{
		public static GameplayController Instance { get; private set; }

		public GameObject TapBlock = null;
		public WinWindow WinWindow = null;
		public LoseWindow LoseWindow = null;


		private int _ordersTarget = 0;

		public int OrdersTarget
		{
			get => _ordersTarget;
			set
			{
				_ordersTarget = value;
				TotalOrdersServedChanged?.Invoke();
			}
		}

		public int TotalOrdersServed { get; private set; } = 0;

		public event Action TotalOrdersServedChanged;

		private void Awake()
		{
			if (Instance != null)
			{
				Debug.LogError("Another instance of GameplayController already exists");
			}

			Instance = this;
		}

		private void OnDestroy()
		{
			if (Instance == this)
			{
				Instance = null;
			}
		}

		private void Init()
		{
			TotalOrdersServed = 0;
			Time.timeScale = 1f;
			TotalOrdersServedChanged?.Invoke();
		}

		public void CheckGameFinish()
		{
			if (CustomersController.Instance.IsComplete)
			{
				EndGame(TotalOrdersServed >= OrdersTarget);
			}
		}

		private void EndGame(bool win)
		{
			Time.timeScale = 0f;
			TapBlock?.SetActive(true);
			if (win)
			{
				WinWindow.Show();
			}
			else
			{
				LoseWindow.Show();
			}
		}

		private void HideWindows()
		{
			TapBlock?.SetActive(false);
			WinWindow?.Hide();
			LoseWindow?.Hide();
		}

		[UsedImplicitly]
		public bool TryServeOrder(Order order)
		{
			if (!CustomersController.Instance.ServeOrder(order))
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
			Init();
			CustomersController.Instance.Initialize();
			HideWindows();

			foreach (var place in FindObjectsOfType<AbstractFoodPlace>())
			{
				place.FreePlace();
			}
		}

		public void CloseGame()
		{
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#else
			Application.Quit();
#endif
		}
	}
}
