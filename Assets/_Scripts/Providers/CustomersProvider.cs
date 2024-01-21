using System.Collections.Generic;
using _Scripts.Kitchen;
using UnityEngine;

namespace _Scripts.Providers
{
    public class CustomersProvider : MonoBehaviour
    {
        [Header("Customers settings:")]
        [Range(1, 100)] public int CustomersTargetNumber = 15;
        [Range(1, 10)] public float CustomerSpawnTime = 3f;
        [Range(5, 50)] public int CustomerWaitTime = 18;
        [Range(1, 10)] public int DecreaseAmountOrdersFromMaximum = 2;
        [Range(1, 10)] public int ReturnTimeAfterGetOrder = 6;

        public List<CustomerPlace> CustomerPlaces;
    }
}