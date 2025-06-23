using System.StateMachineSystem;
using Core.CombatSystem;
using Core.Enemies.States;
using Core.Player.CombatSystem;
using UnityEngine;

namespace Core.Enemies.Components
{
    public class EnemyStateMachine : StateMachineBase<EnemyInstance>, IEnemyState
    {
        public void SetIdle()
        {
            SetState<EnemyStateIdle>();
        }

        public void SetMeleeMoveToTarget(Transform target)
        {
            var state = GetState<EnemyMeleeMoveState>();
            state.Target = target;
            SetState(state);
        }

        public void SetMeleeAttack(IHitBox hitBox)
        {
            var state = GetState<EnemyMeleeAttackState>();
            state.HitBox = hitBox;
            SetState<EnemyMeleeAttackState>();
        }

        public void SetMeleeRigidbodyAttack(IPlayerHitBox hitBox, Collision collision)
        {
            var state = GetState<EnemyMeleeRigidbodyAttackState>();
            state.HitBox = hitBox;
            state.Collision = collision;
            SetState(state);
        }
    }
}