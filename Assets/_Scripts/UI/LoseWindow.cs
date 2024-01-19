using _Scripts.Controllers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI
{
	public sealed class LoseWindow : MonoBehaviour
	{
		public Image GoalBar;
		public TMP_Text GoalText;
		public Button ReplayButton;
		public Button ExitButton;
		public Button CloseButton;

		private bool _isInit = false;

		private void Initialize()
		{
			var gc = GameplayController.Instance;

			ReplayButton.onClick.AddListener(gc.Restart);
			ExitButton.onClick.AddListener(gc.CloseGame);
			CloseButton.onClick.AddListener(gc.CloseGame);
		}

		public void Show()
		{
			if (!_isInit)
			{
				Initialize();
			}

			var gc = GameplayController.Instance;

			GoalBar.fillAmount = Mathf.Clamp01((float)gc.TotalOrdersServed / gc.OrdersTarget);
			GoalText.text = $"{gc.TotalOrdersServed}/{gc.OrdersTarget}";

			gameObject.SetActive(true);
		}

		public void Hide() => 
			gameObject.SetActive(false);
	}
}
