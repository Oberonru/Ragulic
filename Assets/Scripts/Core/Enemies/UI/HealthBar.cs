using System.UI;
using Core.BaseComponents;
using UniRx;
using UnityEngine;

namespace Core.Enemies.UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private FillSlider _slider;
        [SerializeField] private HealthComponent _health;

        private void OnEnable()
        {
            _health?.OnHealthChanged?.Subscribe
                ((value) => _slider.SetSliderValue(_health.CurrentHealth, _health.MaxHealth)).
                AddTo(this);
        }

        private void OnValidate()
        {
            if (_slider is null) GetComponent<FillSlider>();
            if (_health is null) GetComponent<HealthComponent>();
        }
    }
}