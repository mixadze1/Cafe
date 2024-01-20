using _Scripts.Controllers;
using _Scripts.GameLogic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Scripts.UI
{
	public sealed class TopUI : MonoBehaviour
	{
		public Image OrdersBar;
		public TMP_Text OrdersCountText;
		public TMP_Text CustomersCountText;
		
		private CustomersController _customerController;
		private IGameChanges _gameChanges;
		private IGameInfo _gameInfo;

		[Inject]
		private void Construct(CustomersController customersController, IGameChanges gameChanges, IGameInfo gameInfo)
		{
			_gameInfo = gameInfo;
			_gameChanges = gameChanges;
			_customerController = customersController;
		}

		private void Start()
		{
			_gameChanges.TotalOrdersServedChanged += OnOrdersChanged;
			_customerController.TotalCustomersGeneratedChanged += OnCustomersChanged;
			OnOrdersChanged();
			OnCustomersChanged();
		}

		private void OnDestroy()
		{
			if (_gameChanges != null)
				_gameChanges.TotalOrdersServedChanged -= OnOrdersChanged;

			if (_customerController)
				_customerController.TotalCustomersGeneratedChanged -= OnCustomersChanged;
		}

		private void OnCustomersChanged()
		{
			var cc = _customerController;
			CustomersCountText.text = (cc.CustomersTargetNumber - cc.TotalCustomersGenerated).ToString();
		}

		private void OnOrdersChanged()
		{
			OrdersCountText.text = $"{_gameInfo.GetTotalOrdersServed()}/{_gameInfo.GetOrdersTarget()}";
			OrdersBar.fillAmount = (float)_gameInfo.GetTotalOrdersServed() / _gameInfo.GetOrdersTarget();
		}
	}
}
