using Core.BaseComponents;
using Core.CombatSystem;
using Core.Factories;
using Core.Factories.Effects;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using Zenject;

namespace Core.Player.CombatSystem
{
    public class PlayerCombatComponent : CombatComponent
    {
        [Inject] private IEffectFactory _effectFactory;
        [SerializeField, ReadOnly] private PlayerInstance _player;

        public IHitBoxDetector HitBoxDetector => hitBoxDetector;
        [SerializeField, ReadOnly] private TriggerHitBoxDetector hitBoxDetector;

        private IHealthComponent _targetAttack;
        private bool _canAttack;
        private float _time;

        private void OnEnable()
        {
            hitBoxDetector.OnDetected.Subscribe((hitBox) => { _targetAttack = hitBox.HealthComponent; })
                .AddTo(this);

            hitBoxDetector.OnHitBoxExit.Subscribe(_ => { _targetAttack = null; })
                .AddTo(this);
        }

        private void OnValidate()
        {
            if (_player is null) _player = GetComponent<PlayerInstance>();
            if (hitBoxDetector is null) hitBoxDetector = GetComponent<TriggerHitBoxDetector>();
        }

        private void Update()
        {
            _time += Time.deltaTime;

            if (Input.GetMouseButton(0))
            {
                //анимация атаки
            }

            if (Input.GetMouseButton(0) && _targetAttack != null && CanAttack())
            {
                _time = 0;

                RandomDamage(_player.Stats.Damage);

                _targetAttack.TakeDamage(Damage);
            }
        }

        public override void RandomDamage(int damage)
        {
            Damage = Random.Range(damage, damage + 3);
        }
        
        private bool CanAttack()
        {
            return _time > _player.Stats.AttackPerSeconds;
        }
    }
}