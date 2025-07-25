using System.Providers.Configs;
using UnityEngine;

namespace Share.Configs
{
    [CreateAssetMenu(menuName = "Configs/Share/AlarmViewAnimationConfig", fileName = "AlarmViewAnimationConfig")]
    public class AlarmViewAnimationConfig : ScriptableConfig
    {
        [SerializeField] private float _duration;
        [SerializeField] private float _speed;
        
        public float Duration => _duration;
        public float Speed => _speed;
    }
}