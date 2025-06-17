using System.Providers.Configs;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Core.Configs.Enemies
{
    [CreateAssetMenu(menuName = "System/Configs/AIConfig", fileName = "AIConfig")]
    public class AIConfig : ScriptableConfig
    {
        [BoxGroup("Movement", centerLabel: true)]
        [MinValue(0.1f), SuffixLabel("meters")]
        [SerializeField]
        private float _minStoppingDistance = 1.5f;
        
        [BoxGroup("Movement")]
        [MinValue(0.1f), SuffixLabel("meters")]
        [SerializeField]
        private float _maxStoppingDistance = 3f;

        [BoxGroup("Movement")]
        [MinValue(0.1f), SuffixLabel("radius")]
        [SerializeField]
        private float _agressiveRadius = 0.3f;

        [BoxGroup("Movement")]
        [MinValue(1), SuffixLabel("koefficient")]
        [SerializeField]
        private int _agressiveMultiplayer;

        [BoxGroup("Movement")]
        [SuffixLabel("sec")]
        [SerializeField]
        private float _updatedInterval = 0.3f;

        [BoxGroup("Collision")]
        [MinValue(0.1f), SuffixLabel("meters")]
        [SerializeField]
        private float _detectionRadius = 1f;
        

        public float MinStoppingDistance => _minStoppingDistance;
        public float MaxStoppingDistance => _maxStoppingDistance;
        public float AgressiveRadius => _agressiveRadius;
        public float DetectionRadius => _detectionRadius;
        public int AgressiveMultiplayer => _agressiveMultiplayer;
        public float UpdatedInterval => _updatedInterval;
    }
}