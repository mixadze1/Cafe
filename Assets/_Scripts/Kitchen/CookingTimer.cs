using System;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Kitchen
{
	public sealed class CookingTimer : MonoBehaviour
	{
		[Serializable]
		public class TimerSpriteSet
		{
			public Sprite Background = null;
			public Sprite Foreground = null;
		}

		public FoodPlace Place = null;

		public Image Background = null;
		public Image Foreground = null;

		public TimerSpriteSet Normal = null;
		public TimerSpriteSet Overcook = null;

		private TimerSpriteSet _curSet
		{
			set
			{
				if (value == null)
				{
					return;
				}

				if (Background)
				{
					Background.sprite = value.Background;
					Background.SetNativeSize();
				}

				if (Foreground)
				{
					Foreground.sprite = value.Foreground;
					Foreground.SetNativeSize();
				}
			}
		}

		private void Awake()
		{
			if (Place)
			{
				Place.FoodPlaceUpdated += OnFoodPlaceUpdated;
			}
		}

		private void Start()
		{
			OnFoodPlaceUpdated();
		}

		private void Update()
		{
			if (Place == null)
				return;

			if (Place.IsCooking)
			{
				Foreground.fillAmount = Place.TimerNormalized;
			}
		}

		private void OnFoodPlaceUpdated()
		{
			if (Place.IsCooking)
			{
				gameObject.SetActive(true);
				_curSet = Place.CurFood.CurStatus == Food.FoodStatus.Raw ? Normal : Overcook;
			}
			else
			{
				gameObject.SetActive(false);
			}
		}

		private void OnDestroy()
		{
			if (Place)
			{
				Place.FoodPlaceUpdated -= OnFoodPlaceUpdated;
			}
		}
	}
}