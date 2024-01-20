namespace _Scripts.Controllers.Customers
{
    public interface ICustomersInfo
    {   
        int GetTotalCustomersGenerated();
        int GetCustomersTargetNumber();
        float GetCustomerTime();
        int GetTargetOrders();
    }
}