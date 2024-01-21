using System;
using _Scripts.Kitchen;

namespace _Scripts.Controllers.Customers
{
    public interface ICustomersController
    {
        void FreeCustomer(Customer customer);
        bool IsComplete();
        void Initialize();
        bool ServeOrder(Order order);
        event Action TotalCustomersGeneratedChanged;
    }
}