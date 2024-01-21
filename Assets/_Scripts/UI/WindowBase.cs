using UnityEngine;

namespace _Scripts.UI
{
    public abstract class WindowBase : MonoBehaviour
    {
        protected abstract void Show();
        protected abstract void Hide();
    }
}
