using System;
using Core.Models;

namespace Core.Observers
{
    public class PlayerStatisticHandler : IPlayerStatisticHandler
    {
        public IObserver<PlayerStatistic> OnStatisticAdded { get; }
        //откуда брать и загружать статистику
        //когда спавнится, тогда инициализируется PlayerStatistic лист в кастле файте
        //можно сохранить в игре, допустим в джсоне
        private PlayerStatistic _playerStatistic;
        
        public void AddInventoryInteractCount(int value)
        {
            _playerStatistic.AddInteractCunt(value);
        }

        public PlayerStatistic GetStatistic(string playerId)
        {
            if (_playerStatistic.PlayerId == playerId)
            {
                return _playerStatistic;
            }

            return null;
        }
    }
}