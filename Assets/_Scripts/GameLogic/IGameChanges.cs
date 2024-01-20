using System;

namespace _Scripts.GameLogic
{
    public interface IGameChanges
    {
        event Action TotalOrdersServedChanged;
    }
}