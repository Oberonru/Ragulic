using System.Providers.Configs;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Core.Configs.Heart
{
    [CreateAssetMenu(menuName = "Configs/HeartConfig", fileName = "Heart Config")]
    public class HeartConfig : ScriptableConfig
    {
        [BoxGroup("HeartConfig")]
        [PropertyTooltip("Амплитуда в процентах, 0.5 - 50%")]
        [LabelText("Amplitude")]
        [SerializeField]
        private float _amplitude;

        [BoxGroup("HeartConfig")]
        [PropertyTooltip("Минимальная скорость пульсации")]
        [LabelText("MinPulseSpeed")]
        [SerializeField]
        private float _minPulseSpeed;

        [BoxGroup("HeartConfig")]
        [PropertyTooltip("Максимальнач скорость пульсации")]
        [LabelText("MaxPulseSpeed")]
        [SerializeField]
        private float _maxPulseSpeed;

        [BoxGroup("HeartConfig")]
        [PropertyTooltip("Сглаживающий фактор")]
        [LabelText("SmoothFactor")]
        [SerializeField]
        private float _smoothFactor;
        
        [BoxGroup("HeartConfig")]
        [PropertyTooltip("Радиус в котором отслеживается сердцебиение")]
        [LabelText("Detected radius")]
        [SerializeField]
        private float _detectedRadius;

        [BoxGroup("HeartConfig")]
        [PropertyTooltip("Звук удара сердца")]
        [LabelText("Beat")]
        [SerializeField]
        private AudioClip _beatSound;

        public float Amplitude => _amplitude;
        public float MinPulseSpeed => _minPulseSpeed;
        public float MaxPulseSpeed => _maxPulseSpeed;
        public float SmoothFactor => _smoothFactor;
        public float DetectedRadius => _detectedRadius;
        public AudioClip BeatSound => _beatSound;
    }
}