using _Scripts.GameLogic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Scripts.UI
{
	public sealed class WinWindow : WindowBase
	{
		private IGameDelegates _gameDelegates;
		private IGameHandler _gameHandler;
		private IGameInfo _gameInfo;

		public Image GoalBar;
		public TMP_Text GoalText;
		public Button OkButton;
		public Button CloseButton;

		[Inject]
		private void Construct(IGameHandler gameHandler, IGameDelegates gameDelegates, IGameInfo gameInfo)
		{
			_gameDelegates = gameDelegates;
			_gameInfo = gameInfo;
			_gameHandler = gameHandler;
			_gameDelegates.OnWinGame += Show;
			_gameDelegates.OnRestartGame += Hide;
			
			OkButton.onClick.AddListener(_gameHandler.CloseGame);
			CloseButton.onClick.AddListener(_gameHandler.CloseGame);
		}

		protected override void Show()
		{
			GoalText.text = $"{_gameInfo.GetTotalOrdersServed()}/{_gameInfo.GetOrdersTarget()}";
			GoalBar.fillAmount = Mathf.Clamp01((float)_gameInfo.GetTotalOrdersServed() / _gameInfo.GetOrdersTarget());
			
			gameObject.SetActive(true);
		}

		protected override void Hide() => 
			gameObject.SetActive(false);

		private void OnDestroy()
		{
			_gameDelegates.OnWinGame -= Show;
			_gameDelegates.OnRestartGame -= Hide;
		}
	}
}
