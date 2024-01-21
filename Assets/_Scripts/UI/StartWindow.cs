using _Scripts.GameLogic;
using _Scripts.Pause;
using TMPro;
using UnityEngine.UI;
using Zenject;

namespace _Scripts.UI
{
    public class StartWindow : WindowBase
    {
        private IGameInfo _gameInfo;
        private IGameDelegates _gameDelegates;
        private IPauseProvider _pauseProvider;

        public TMP_Text GoalText;

        public Button ExitButton;
        public Button OkayButton;

        [Inject]
        private void Construct(IGameDelegates gameDelegates, IGameInfo gameInfo, IPauseProvider pauseProvider)
        {
            _pauseProvider = pauseProvider;
            _gameDelegates = gameDelegates;
            _gameInfo = gameInfo;

            _gameDelegates.OnStartGame += Show;

            ExitButton.onClick.AddListener(Hide);
            OkayButton.onClick.AddListener(Hide);
        }

        protected override void Show()  
        {
            _pauseProvider.PauseGame();
            GoalText.text = $"{_gameInfo.GetOrdersTarget()}";

            gameObject.SetActive(true);
        }

        protected override void Hide()
        {
            _pauseProvider.UnPauseGame();
            gameObject.SetActive(false);
        }
    }
}
