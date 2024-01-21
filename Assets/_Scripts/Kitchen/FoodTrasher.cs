using UnityEngine;
using Zenject;

namespace _Scripts.Kitchen
{
	[RequireComponent(typeof(FoodPlace))]
	public sealed class FoodTrasher : MonoBehaviour
	{
		private TrashHandler _trashHandler;
		private FoodPlace _place;
		
		private float _fixedTime;
		private float _duration = 0.25f;

		private  bool _isDoubleTap;

		[Inject]
		private void Construct(TrashHandler trashHandler)
		{
			_trashHandler = trashHandler;
			_place = GetComponent<FoodPlace>();
			_fixedTime = 0;
		}

		private void FixedUpdate()
		{
			if (!_isDoubleTap)
				return;

			_fixedTime += Time.fixedDeltaTime;
			if (_fixedTime >= _duration)
			{
				_fixedTime = 0;
				_isDoubleTap = false;
			}
		}

		public void TryTrashFood()
		{
			if (!_isDoubleTap)
			{
				_isDoubleTap = true;
				return;
			}

			if (!_place.IsFree && _place.CurFood.CurStatus == Food.FoodStatus.Overcooked)
				_trashHandler.ThrowTrash(_place);

		}
	}
}
