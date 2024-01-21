using System;

namespace _Scripts.Pause
{
    public interface IPauseProvider
    {
        public void PauseGame();

        public void UnPauseGame();
        public event Action OnPauseGame;
        public event Action OnUnPauseGame;
    }
}