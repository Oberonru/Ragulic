using System.Collections.Generic;
using System.Providers.Configs;
using Core.Observers;
using UnityEngine;

namespace Core.Configs.Observers
{
    [CreateAssetMenu(menuName = "Configs/Observers/Game Statistic Spawner Config",
        fileName = "Game Statistic Spawner Config")]
    public class ObserverGameStatisticSpawnerConfig : ScriptableConfig
    {
        [SerializeField] private PlayerStatisticObserver[] _observers;

        public IEnumerable<PlayerStatisticObserver> Observers => _observers;
    }
}