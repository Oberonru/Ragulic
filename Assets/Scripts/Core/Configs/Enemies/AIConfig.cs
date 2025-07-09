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

        [BoxGroup("Detected")]
        [PropertyTooltip("Радиус на безусловного обнаружения игрока агентом")]
        [MinValue(0.1f), SuffixLabel("meters")]
        [SerializeField]
        private float _detectionRadius = 1f;

        [BoxGroup("Detected")]
        [PropertyTooltip("Величина луча на котором видит агент")]
        [MinValue(0.1f), SuffixLabel("meters")]
        [SerializeField]
        private float _seeDistance = 5f;

        [BoxGroup("Detected")]
        [LabelText("Min field of view")]
        [PropertyTooltip("Минимальный угол обзора")]
        [SuffixLabel("gradus")]
        [SerializeField]
        private float _minFOV = -80f;
        
        [BoxGroup("Detected")]
        [LabelText("Max field of view")]
        [PropertyTooltip("Максимальный угол обзора")]
        [SuffixLabel("gradus")]
        [SerializeField]
        private float _maxFOV = 80f;

        public float MinStoppingDistance => _minStoppingDistance;
        public float MaxStoppingDistance => _maxStoppingDistance;
        public float AgressiveRadius => _agressiveRadius;
        public float DetectionRadius => _detectionRadius;
        public int AgressiveMultiplayer => _agressiveMultiplayer;
        public float UpdatedInterval => _updatedInterval;
        public float SeeDistance => _seeDistance;
        public float MinFOV => _minFOV;
        public float MaxFOV => _maxFOV;
    }
}