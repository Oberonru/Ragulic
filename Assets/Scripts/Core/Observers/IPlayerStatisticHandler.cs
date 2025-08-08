using System;
using Core.Models;

namespace Core.Observers
{
    public interface IPlayerStatisticHandler
    {
        IObserver<PlayerStatistic> OnStatisticAdded { get; }
        
        void AddInventoryInteractCount(int count);
        
        PlayerStatistic GetStatistic(string playerId);
    }
}