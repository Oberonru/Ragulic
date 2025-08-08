using Core.Observers;
using Core.Player;
using UniRx;
using Zenject;

namespace Core.Items.Observers
{
    public class InventoryInteractStatisticsObserver : PlayerStatisticObserver
    {
        [Inject] private IPlayerInstance _player;
        
        private void Awake()
        {
            _player.ItemHandler.OnInteract.Subscribe(_ =>
            {
                StatisticHandler.AddInventoryInteractCount(1);
            }).AddTo(this);
        }
    }
}