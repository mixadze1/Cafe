using UnityEngine;
using Zenject;

namespace _Scripts.GameLogic
{
   public class TapBlock : MonoBehaviour
   {
      [SerializeField] private RectTransform _container;
      private IGameHandler _gameHandler;

      [Inject]
      public void Construct(IGameHandler gameHandler)
      {
         _gameHandler = gameHandler;
         _gameHandler.OnEndGame += Show;
         _gameHandler.OnRestartGame += Hide;
      }

      private void Show() => 
         _container.gameObject.SetActive(true);

      private void Hide() => 
         _container.gameObject.SetActive(false);

      private void OnDestroy()
      {
         _gameHandler.OnEndGame -= Show;
         _gameHandler.OnRestartGame -= Hide;
      }
   }
}
