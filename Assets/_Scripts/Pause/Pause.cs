using System;
using UnityEngine;

namespace _Scripts.Pause
{
    public class Pause : IPauseProvider
    {
        public event Action OnPauseGame;
        public event Action OnUnPauseGame;

        public void PauseGame()
        {
            Time.timeScale = 0f;
            OnPauseGame?.Invoke();
        }


        public void UnPauseGame()
        {
            Time.timeScale = 1f;
            OnUnPauseGame?.Invoke();
        }
    }
}