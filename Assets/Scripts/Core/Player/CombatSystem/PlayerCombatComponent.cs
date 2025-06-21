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
        private bool _canAttack;
        private float _time;

        private void OnEnable()
        {
            _detector.OnDetected.Subscribe((hitBox) => 
                { _healthComponent = hitBox.HealthComponent; }).AddTo(this);

            _detector.OnHitBoxExit.Subscribe(_ => 
                { _healthComponent = null; }).AddTo(this);
        }

        private void Update()
        {
            _time += Time.deltaTime;

            if (Input.GetMouseButton(0))
            {
                //анимация атаки
            }

            if (Input.GetMouseButton(0) && _healthComponent != null && CanAttack())
            {
                _time = 0;

                SetRandomDamage(_player.Stats.Damage);

                _healthComponent.TakeDamage(Damage);
            }
        }

        private void OnValidate()
        {
            if (_player is null) _player = GetComponent<PlayerInstance>();
        }

        public override void SetRandomDamage(int damage)
        {
            Damage = Random.Range(damage, damage + 3);
        }

        private bool CanAttack()
        {
            return _time > _player.Stats.AttackPerSeconds;
        }
    }
}