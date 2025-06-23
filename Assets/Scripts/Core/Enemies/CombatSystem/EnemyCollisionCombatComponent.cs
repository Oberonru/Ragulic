using Core.CombatSystem;
using Core.Player;
using Core.Player.CombatSystem;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using Zenject;

namespace Core.Enemies.CombatSystem
{
    public class EnemyCollisionCombatComponent : CombatComponent
    {
        [Inject] private IPlayerInstance _player;
        [SerializeField, ReadOnly] private EnemyInstance _enemyInstance;
        [SerializeField, ReadOnly] private EnemyCollisionHitBoxDetector _collisionDetector;

        private void OnEnable()
        {
            _collisionDetector.OnHitBoxDetected.Subscribe((hitBox =>
            {
                if (hitBox is IPlayerHitBox playerHitBox)
                {
                    Attack(playerHitBox);
                }
            })).AddTo(this);

            _collisionDetector.OnHitBoxExit.Subscribe((_) =>
                _enemyInstance.StateMachine.SetMeleeMoveToTarget(_player.Transform)).AddTo(this);
        }

        private void Attack(IPlayerHitBox playerHitBox)
        {
            _collisionDetector.OnCollisionDetected?.Subscribe((collision) =>
            {
                _enemyInstance.StateMachine.SetMeleeRigidbodyAttack(playerHitBox, collision);
            }).AddTo(this);
            
           
        }

        public override void SetRandomDamage(int damage)
        {
        }

        private void OnValidate()
        {
            if (_enemyInstance is null) _enemyInstance = GetComponent<EnemyInstance>();
            if (_collisionDetector is null) _collisionDetector = GetComponent<EnemyCollisionHitBoxDetector>();
        }
    }
}