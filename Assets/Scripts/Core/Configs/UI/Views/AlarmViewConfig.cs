using System.Providers.Configs;
using System.UI.AlarmSystem.Views;
using UnityEngine;

namespace Core.Configs.UI.Views
{
    [CreateAssetMenu(menuName = "Configs/UI/AlarmViewConfig", fileName = "AlarmViewConfig")]

    public class AlarmViewConfig : ScriptableConfig
    {
        [SerializeField] private AlarmView _prefab;
        [SerializeField] private int _poolSize = 20;

        public AlarmView Prefab => _prefab;
        public int PoolSize => _poolSize;
    }
}