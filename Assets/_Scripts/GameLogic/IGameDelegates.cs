using System;

namespace _Scripts.GameLogic
{
    public interface IGameDelegates
    {
        public event Action OnWinGame;
        public event Action OnLoseGame;
        public event Action OnEndGame;
        public event Action OnRestartGame;
        public event Action OnStartGame;
    }
}