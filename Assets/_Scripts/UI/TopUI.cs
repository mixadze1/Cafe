using _Scripts.Controllers;
using _Scripts.Controllers.Customers;
using _Scripts.GameLogic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Scripts.UI
{
	public sealed class TopUI : MonoBehaviour
	{
		private ICustomersInfo _customersInfo;
		private IGameChanges _gameChanges;
		private IGameInfo _gameInfo;
		private IGameDelegates _gameDelegates;
		private ICustomersController _customersesController;

		public Image OrdersBar;
		public TMP_Text OrdersCountText;
		public TMP_Text CustomersCountText;

		[Inject]
		private void Construct(ICustomersInfo customerInfo, IGameDelegates gameDelegates, ICustomersController customersController, IGameChanges gameChanges, IGameInfo gameInfo)
		{
			_gameDelegates = gameDelegates;
			_gameInfo = gameInfo;
			_gameChanges = gameChanges;
			_customersInfo = customerInfo;
			_customersesController = customersController;
			_gameDelegates.OnStartGame += OnStartGame;
		}

		private void OnStartGame()
		{
			_gameChanges.TotalOrdersServedChanged += OnOrdersChanged;
			_customersesController.TotalCustomersGeneratedChanged += OnCustomersesChanged;
			OnOrdersChanged();
			OnCustomersesChanged();
		}

		private void OnDestroy()
		{
			if (_gameChanges != null)
				_gameChanges.TotalOrdersServedChanged -= OnOrdersChanged;

			if (_customersInfo != null)
				_customersesController.TotalCustomersGeneratedChanged -= OnCustomersesChanged;
			
			if(_gameDelegates != null)
				_gameDelegates.OnStartGame -= OnStartGame;
		}

		private void OnCustomersesChanged()
		{
			var cc = _customersInfo;
			CustomersCountText.text = (cc.GetCustomersTargetNumber() - cc.GetTotalCustomersGenerated()).ToString();
		}

		private void OnOrdersChanged()
		{
			OrdersCountText.text = $"{_gameInfo.GetTotalOrdersServed()}/{_gameInfo.GetOrdersTarget()}";
			OrdersBar.fillAmount = (float)_gameInfo.GetTotalOrdersServed() / _gameInfo.GetOrdersTarget();
		}
	}
}
