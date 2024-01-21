using _Scripts.GameLogic;
using _Scripts.Pause;
using UnityEngine;
using Zenject;

namespace _Scripts.UI
{
   public class TapBlock : MonoBehaviour
   {
      [SerializeField] private RectTransform _container;
      private IGameDelegates _gameDelegates;
      private IPauseProvider _pauseProvider;

      [Inject]
      public void Construct(IGameDelegates gameDelegates, IPauseProvider pauseProvider)
      {
         _pauseProvider = pauseProvider;
         _gameDelegates = gameDelegates;
         _gameDelegates.OnEndGame += Show;
         _gameDelegates.OnRestartGame += Hide;

         _pauseProvider.OnPauseGame += Show;
         _pauseProvider.OnUnPauseGame += Hide;
      }

      private void Show() => 
         _container.gameObject.SetActive(true);

      private void Hide() => 
         _container.gameObject.SetActive(false);

      private void OnDestroy()
      {
         _gameDelegates.OnEndGame -= Show;
         _gameDelegates.OnRestartGame -= Hide;
         
         _pauseProvider.OnPauseGame -= Show;
         _pauseProvider.OnUnPauseGame -= Hide;
      }
   }
}
