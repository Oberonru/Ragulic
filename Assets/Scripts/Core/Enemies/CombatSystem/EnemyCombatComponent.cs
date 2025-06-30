using System;
using Core.CombatSystem;
using Core.Player;
using Core.Player.CombatSystem;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using Zenject;

namespace Core.Enemies.CombatSystem
{
    public class EnemyCombatComponent : CombatComponent
    {
        [Inject] private IPlayerInstance _player;
        [SerializeField, ReadOnly] private EnemyInstance _enemyInstance;
        [SerializeField, ReadOnly] private EnemyCollisionHitBoxDetector _detector;

        private bool _isAttacking;

        private void OnEnable()
        {
            _detector.OnDetected.Subscribe(hitBox =>
            {
                if (_isAttacking) return;

                Attack(hitBox);
            });

            _detector.OnHitBoxExit.Subscribe((_) =>
                _enemyInstance.StateMachine.SetMeleeMoveToTarget(_player.Transform)).AddTo(this);
        }

        private void OnValidate()
        {
            if (_enemyInstance is null) _enemyInstance = GetComponent<EnemyInstance>();
            if (_detector is null) _detector = GetComponent<EnemyCollisionHitBoxDetector>();
        }

        private async UniTask Attack(IHitBox hitBox)
        {
            if (!(hitBox is IPlayerHitBox) || !hitBox.HealthComponent.IsAllive) return;
            
            Debug.Log("Attack");
            
            _isAttacking = true;
            
            var span = TimeSpan.FromSeconds(_enemyInstance.EnemyStats.AttackPerSeconds);
            _enemyInstance.StateMachine.SetMeleeAttack(hitBox);

            await UniTask.Delay(span);
            
            _isAttacking = false;
        }
    }
}