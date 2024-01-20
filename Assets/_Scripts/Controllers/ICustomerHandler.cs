using _Scripts.Kitchen;

namespace _Scripts.Controllers
{
    public interface ICustomerHandler
    {
        float GetCustomerTime();
        void FreeCustomer(Customer customer);
        bool IsComplete();
    }
}