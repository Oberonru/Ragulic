using Core.CombatSystem;
using Core.Player;
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
        public ISubject<Unit> OnAttack => _onAttack;
        private Subject<Unit> _onAttack = new();

        private void OnEnable()
        {
            _detector.OnDetected.Subscribe(dto =>
            {
                _enemyInstance.StateMachine.SetMeleeAttack(dto.HitBox, dto.Collision);
                _onAttack?.OnNext(Unit.Default);
            });

            _detector.OnHitBoxExit.Subscribe((_) =>
                _enemyInstance.StateMachine.SetMeleeMoveToTarget(_player.Transform)).AddTo(this);
        }

        private void OnValidate()
        {
            if (_enemyInstance is null) _enemyInstance = GetComponent<EnemyInstance>();
            if (_detector is null) _detector = GetComponent<EnemyCollisionHitBoxDetector>();
        }
    }
}