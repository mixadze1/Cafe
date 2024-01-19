using _Scripts.Controllers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI
{
	public sealed class WinWindow : MonoBehaviour
	{
		private bool _isInit = false;
		
		public Image GoalBar;
		public TMP_Text GoalText;
		public Button OkButton;
		public Button CloseButton;


		private void Initialize()
		{
			var gc = GameplayController.Instance;

			OkButton.onClick.AddListener(gc.CloseGame);
			CloseButton.onClick.AddListener(gc.CloseGame);
		}

		public void Show()
		{
			if (!_isInit)
			{
				Initialize();
			}

			var gc = GameplayController.Instance;

			GoalText.text = $"{gc.TotalOrdersServed}/{gc.OrdersTarget}";
			GoalBar.fillAmount = Mathf.Clamp01((float)gc.TotalOrdersServed / gc.OrdersTarget);

			gameObject.SetActive(true);
		}

		public void Hide() => 
			gameObject.SetActive(false);
	}
}
