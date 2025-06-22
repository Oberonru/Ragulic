using Core.CombatSystem;
using Core.Player.CombatSystem;
using Sirenix.OdinInspector;
using UnityEngine;
using UniRx;

namespace Core.Enemies.CombatSystem
{
    public class EnemyColliderCombatComponent : CombatComponent
    {
        [SerializeField, ReadOnly] private EnemyInstance _enemyInstance;
        [SerializeField] private EnemyColliderHitBoxDetector _colliderDetector;

        private void OnEnable()
        {
            _colliderDetector.OnDetected.Subscribe((hitBox)
                =>
            {
                if (hitBox is IPlayerHitBox)
                {
                    _enemyInstance.StateMachine.SetMeleeRigidbodyAttack(hitBox);
                }
            }).AddTo(this);
        }

        public override void SetRandomDamage(int damage)
        {
        }

        private void OnValidate()
        {
            if (_enemyInstance is null) _enemyInstance = GetComponent<EnemyInstance>();
            if (_colliderDetector is null) _colliderDetector = GetComponent<EnemyColliderHitBoxDetector>();
        }
    }
}