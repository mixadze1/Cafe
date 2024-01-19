using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Kitchen
{
	public sealed class FoodVisualizer : MonoBehaviour
	{
		public string Name;
		public List<GameObject> AuxObjects = new();

		public void SetEnabled(bool yesno)
		{
			gameObject.SetActive(yesno);
			AuxObjects.ForEach(x => x.SetActive(yesno));
		}
	}
}
