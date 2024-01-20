using System.Collections.Generic;
using _Scripts.Kitchen;
using UnityEngine;

namespace _Scripts.GameLogic
{
    public class CustomersProvider : MonoBehaviour
    {
        public int CustomersTargetNumber = 15;
        public float CustomerSpawnTime = 3f;

        public List<CustomerPlace> CustomerPlaces;
    }
}