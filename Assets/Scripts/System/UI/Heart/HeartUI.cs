using Core.Configs.Heart;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace System.UI.Heart
{
    public class HeartUI : MonoBehaviour
    {
        [Inject] private HeartConfig _config;
        [SerializeField] private Image _heart;
        public float DetectedRadius => _detectedRadius;
        private float _detectedRadius => _config.DetectedRadius;
        
        private float _frequency;
        private Vector3 _startScale;
        private float _phase;

        private void OnEnable()
        {
            if (_heart is null) _heart = GetComponent<Image>();

            _startScale = _heart.transform.localScale;
        }

        public void Beat(float distance, float detectedRadius)
        {
            var currentPositionPercent = Mathf.InverseLerp(detectedRadius, 0, distance);
            var targetFrequency = Mathf.Lerp(_config.MinPulseSpeed, _config.MaxPulseSpeed, currentPositionPercent);

            _frequency = Mathf.Lerp(_frequency, targetFrequency, Time.deltaTime * _config.SmoothFactor);

            _phase += _frequency * Time.deltaTime;

            if (_phase > 1f) _phase -= 1f;

            var scaleKoef = 1 + _config.Amplitude * Mathf.Sin(_phase * Mathf.PI * 2);
            transform.localScale = _startScale * scaleKoef;
        }
    }
}