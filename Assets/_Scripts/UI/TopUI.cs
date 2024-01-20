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
		private ICustomerHandler _customersHandler;
		
		public Image OrdersBar;
		public TMP_Text OrdersCountText;
		public TMP_Text CustomersCountText;
		
		[Inject]
		private void Construct(ICustomersInfo customerInfo, ICustomerHandler customerHandler, IGameChanges gameChanges, IGameInfo gameInfo)
		{
			_gameInfo = gameInfo;
			_gameChanges = gameChanges;
			_customersInfo = customerInfo;
			_customersHandler = customerHandler;
		}

		private void Start()
		{
			_gameChanges.TotalOrdersServedChanged += OnOrdersChanged;
			_customersHandler.TotalCustomersGeneratedChanged += OnCustomersChanged;
			OnOrdersChanged();
			OnCustomersChanged();
		}

		private void OnDestroy()
		{
			if (_gameChanges != null)
				_gameChanges.TotalOrdersServedChanged -= OnOrdersChanged;

			if (_customersInfo != null)
				_customersHandler.TotalCustomersGeneratedChanged -= OnCustomersChanged;
		}

		private void OnCustomersChanged()
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
