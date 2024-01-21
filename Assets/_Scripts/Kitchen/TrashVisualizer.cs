using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Scripts.Kitchen
{
    public class TrashVisualizer : MonoBehaviour
    {
        private TrashHandler _trashHandler;

        private bool _isThrow;
        private float _duration = 0.25f;
        private float _fixedTime;

        public Image Icon;
        public Sprite Close;
        public Sprite Open;

        [Inject]
        private void Construct(TrashHandler trashHandler)
        {
            _trashHandler = trashHandler;
            _trashHandler.OnThrowTrash += OpenTrash;
        }

        private void FixedUpdate()
        {
            if (!IsOpen())
                return;

            _fixedTime += Time.fixedDeltaTime;
            if (_fixedTime >= _duration)
            {
                _fixedTime = 0;
                CloseTrash();
            }
        }

        private void OpenTrash()
        {
            Icon.sprite = Open;
            _isThrow = true;
        }

        private void CloseTrash()
        {
            Icon.sprite = Close;
            _isThrow = false;
        }

        private bool IsOpen() =>
            _isThrow;

        private void OnDestroy() =>
            _trashHandler.OnThrowTrash -= OpenTrash;
    }
}