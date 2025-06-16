using Core.BaseComponents;
using UniRx;
using UnityEngine;

namespace System.UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private FillSlider _slider;
        [SerializeField] private IHealthComponent _health;

        private void OnValidate()
        {
            if (_slider is null) GetComponent<FillSlider>();
            if (_health is null) GetComponent<HealthComponent>();
        }

        public void SetData(IHealthComponent health)
        {
            _health = health;
            SubscribeOnHealthChanged();   
        }

        private void SubscribeOnHealthChanged()
        {
            _health?.OnHealthChanged?.Subscribe
                    ((value) => _slider.SetSliderValue(_health.CurrentHealth, _health.MaxHealth)).
                AddTo(this);
        }
    }
}