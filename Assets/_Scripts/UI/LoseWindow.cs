using System;
using _Scripts.Controllers;
using _Scripts.GameLogic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Scripts.UI
{
	public sealed class LoseWindow : MonoBehaviour
	{
		private IGameHandler _gameHandler;
		private IGameInfo _gameInfo;

		public Image GoalBar;
		public TMP_Text GoalText;
		public Button ReplayButton;
		public Button ExitButton;
		public Button CloseButton;


		[Inject]
		private void Construct(IGameHandler gameHandler, IGameInfo gameInfo)
		{
			_gameHandler = gameHandler;
			_gameInfo = gameInfo;

			_gameHandler.OnLoseGame += Show;
			_gameHandler.OnRestartGame += Hide;

			ReplayButton.onClick.AddListener(_gameHandler.Restart);
			ExitButton.onClick.AddListener(_gameHandler.CloseGame);
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
			_gameHandler.OnLoseGame -= Show;
			_gameHandler.OnRestartGame -= Hide;
		}
	}
}
