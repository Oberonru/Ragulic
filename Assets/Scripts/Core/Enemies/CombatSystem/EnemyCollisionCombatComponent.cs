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
        [SerializeField, ReadOnly] private EnemyCollisionHitBoxDetector _detector;
        public ISubject<Unit> OnAttack => _onAttack;
        private Subject<Unit> _onAttack = new();

        private void OnEnable()
        {
            _detector.OnHitBoxDetected.Subscribe((hitBox =>
            {
                if (hitBox is IPlayerHitBox playerHitBox)
                {
                    Attack(playerHitBox);
                    _onAttack.OnNext(Unit.Default);
                }
            })).AddTo(this);

            _detector.OnHitBoxExit.Subscribe((_) =>
                _enemyInstance.StateMachine.SetMeleeMoveToTarget(_player.Transform)).AddTo(this);
        }

        private void Attack(IPlayerHitBox playerHitBox)
        {
            _detector.OnCollisionDetected?.Subscribe((collision) =>
            {
                _enemyInstance.StateMachine.SetMeleeAttack(playerHitBox, collision);
            }).AddTo(this);
        }

        public override void SetRandomDamage(int damage)
        {
        }

        private void OnValidate()
        {
            if (_enemyInstance is null) _enemyInstance = GetComponent<EnemyInstance>();
            if (_detector is null) _detector = GetComponent<EnemyCollisionHitBoxDetector>();
        }
    }
}