using System;
using _Scripts.Controllers;
using _Scripts.GameLogic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Scripts.UI
{
	public sealed class WinWindow : MonoBehaviour
	{
		private IGameHandler _gameHandler;
		private IGameInfo _gameInfo;

		public Image GoalBar;
		public TMP_Text GoalText;
		public Button OkButton;
		public Button CloseButton;

		[Inject]
		private void Construct(IGameHandler gameHandler, IGameInfo gameInfo)
		{
			_gameInfo = gameInfo;
			_gameHandler = gameHandler;
			_gameHandler.OnWinGame += Show;
			_gameHandler.OnRestartGame += Hide;
			
			OkButton.onClick.AddListener(_gameHandler.CloseGame);
			CloseButton.onClick.AddListener(_gameHandler.CloseGame);
		}

		private void Show()
		{
			GoalText.text = $"{_gameInfo.GetTotalOrdersServed()}/{_gameInfo.GetOrdersTarget()}";
			GoalBar.fillAmount = Mathf.Clamp01((float)_gameInfo.GetTotalOrdersServed() / _gameInfo.GetOrdersTarget());

			gameObject.SetActive(true);
		}

		public void Hide() => 
			gameObject.SetActive(false);

		private void OnDestroy()
		{
			_gameHandler.OnWinGame -= Show;
			_gameHandler.OnRestartGame -= Hide;
		}
	}
}
