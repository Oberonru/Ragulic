using System.Providers.Configs;
using UnityEngine;
using DG.Tweening;

namespace Share.Configs
{
    [CreateAssetMenu(menuName = "Configs/Share/AlarmViewAnimationConfig", fileName = "AlarmViewAnimationConfig")]
    public class AlarmViewAnimationConfig : ScriptableConfig
    {
        [SerializeField] private float _duration = 3f;
        [SerializeField] private float _speed;
        [SerializeField] private float _delayDeactivate = 0.08f;
        [SerializeField] private Ease _ease = Ease.Linear;

        public float Duration => _duration;
        public float Speed => _speed;
        public float DelayDeactivate => _delayDeactivate;
        public Ease Ease => _ease;
    }
}