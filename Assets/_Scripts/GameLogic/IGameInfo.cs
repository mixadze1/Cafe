using _Scripts.Kitchen;

namespace _Scripts.GameLogic
{
    public interface IGameInfo
    {
        int GetOrdersTarget();
        int GetTotalOrdersServed();
        bool TryServeOrder(Order order);
    }
}