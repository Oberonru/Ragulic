using Core.CombatSystem;
using Core.Player;
using Core.Player.CombatSystem;
using UniRx;
using UnityEngine;
using Zenject;

namespace Core.Enemies.CombatSystem
{
    public class EnemyCombatComponent : CombatComponent
    {
        [Inject] private IPlayerInstance _player;
        [SerializeField] private EnemyInstance _enemyInstance;
        [SerializeField] private EnemyHitBoxDetector _detector;


        private void OnEnable()
        {
            _detector.OnDetected.Subscribe((hitBox) =>
                {
                    if (hitBox is PlayerHitBox playerHitBox)
                    {
                        _enemyInstance.StateMachine.SetMeleeAttack(hitBox);
                    }
                })
                .AddTo(this);

            _detector.OnHitBoxExit.Subscribe(hitBox =>
                _enemyInstance.StateMachine.SetMeleeMoveToTarget(_player.Transform)).AddTo(this);
        }

        private void Start()
        {
            SetDamage(_enemyInstance.EnemyStats.Damage);
        }

        private void OnValidate()
        {
            if (_enemyInstance is null) GetComponent<EnemyInstance>();
            if (_detector is null) _detector = GetComponent<EnemyHitBoxDetector>();
        }
    }
}