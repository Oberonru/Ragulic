using Core.BaseComponents;
using Core.CombatSystem;
using UniRx;
using UnityEngine;

namespace Core.Player.CombatSystem
{
    public class PlayerCombatComponent : CombatComponent
    {
        [SerializeField] private PlayerInstance _player;
        [SerializeField] private HitBoxDetector _detector;
        private IHealthComponent _healthComponent;

        private void OnEnable()
        {
            _detector.OnDetected.Subscribe((hitBox) => { _healthComponent = hitBox.HealthComponent; }).AddTo(this);

            _detector.OnHitBoxExit.Subscribe(_ => { _healthComponent = null; }).AddTo(this);
        }

        private void Start()
        {
            SetDamage(_player.Stats.Damage);
        }

        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                //анимация атаки
            }

            if (Input.GetMouseButton(0) && _healthComponent != null)
            {
                _healthComponent.TakeDamage(Damage);
            }
        }

        private void OnValidate()
        {
            if (_player is null) _player = GetComponent<PlayerInstance>();
        }
    }
}