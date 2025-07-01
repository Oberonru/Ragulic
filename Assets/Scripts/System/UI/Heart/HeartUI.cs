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
        [SerializeField] private AudioSource _audio;
        public float DetectedRadius => _detectedRadius;
        private float _detectedRadius => _config.DetectedRadius;

        private Vector3 _startScale;
        private float _frequency;
        private float _phase;
        private float _lastPhase;
        private bool _canPlayBeat = true;

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

            var sin = Mathf.Sin(_phase * Mathf.PI * 2);
            var scaleKoef = 1 + _config.Amplitude * sin;

            transform.localScale = _startScale * scaleKoef;

            BeatSound(_phase);
        }

        private void BeatSound(float phase)
        {
            if (_lastPhase < 0.25f && phase >= 0.25f && _canPlayBeat)
            {
                _canPlayBeat = false;
                _audio.pitch = UnityEngine.Random.Range(0.8f, 1.1f);
                _audio.PlayOneShot(_config.BeatSound);
            }

            _lastPhase = _phase;
            _canPlayBeat = true;
        }
    }
}