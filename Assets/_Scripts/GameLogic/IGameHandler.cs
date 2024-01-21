using System;

namespace _Scripts.GameLogic
{
    public interface IGameHandler
    {
        void CloseGame();
        void Restart();
        void CheckGameFinish();
    }
}