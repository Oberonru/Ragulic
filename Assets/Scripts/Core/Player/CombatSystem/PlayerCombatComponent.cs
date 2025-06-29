using Core.BaseComponents;
using Core.CombatSystem;
using Core.Effects;
using Core.Factories;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using Zenject;

namespace Core.Player.CombatSystem
{
    public class PlayerCombatComponent : CombatComponent
    {
      //  [Inject] private IEffectFactory _effectFactory;
        [SerializeField, ReadOnly] private PlayerInstance _player;
        [SerializeField, ReadOnly] private TriggerHitBoxDetector _detector;
        [SerializeField] private EffectInstance _particalEffect;
        private IHealthComponent _targetAttack;
        private bool _canAttack;
        private float _time;

        private void OnEnable()
        {
            _detector.OnDetected.Subscribe((hitBox) => { _targetAttack = hitBox.HealthComponent; })
                .AddTo(this);

            _detector.OnHitBoxExit.Subscribe(_ => { _targetAttack = null; })
                .AddTo(this);
        }

        private void OnValidate()
        {
            if (_player is null) _player = GetComponent<PlayerInstance>();
            if (_detector is null) _detector = GetComponent<TriggerHitBoxDetector>();
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
                //эффект крови допустим
                // _effectFactory.Create(
                //     _particalEffect, _player.Transform.position, _player.Transform.rotation, _player.Transform);

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