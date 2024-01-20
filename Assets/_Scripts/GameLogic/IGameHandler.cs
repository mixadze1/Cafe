using System;

namespace _Scripts.GameLogic
{
    public interface IGameHandler
    {
        public event Action OnWinGame;
        public event Action OnLoseGame;
        public event Action OnEndGame;
        public event Action OnRestartGame;

        void CloseGame();
        void Restart();
        void CheckGameFinish();
    }
}