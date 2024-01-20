using System;
using System.Collections.Generic;
using _Scripts.Kitchen;

namespace _Scripts.Controllers.Orders
{
    public interface IOrdersHandler
    {
        void Initialize();
        Order FindOrder(List<string> foods);
        event Action OnCompleteOrder;
    }
}