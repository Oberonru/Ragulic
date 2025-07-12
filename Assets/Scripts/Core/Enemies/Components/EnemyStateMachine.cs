using System.StateMachineSystem;
using Core.CombatSystem;
using Core.Enemies.States;
using UnityEngine;

namespace Core.Enemies.Components
{
    public class EnemyStateMachine : StateMachineBase<EnemyInstance>, IEnemyState
    {
        public void SetIdle()
        {
            var state = GetState<EnemyStateIdle>();
            SetState(state);
        }

        public void SetSearchPlayer()
        {
            SetState<EnemySearchPlayer>();
        }

        public void SetMeleeMoveToTarget(Transform target)
        {
            var state = GetState<EnemyMeleeMoveState>();
            state.Target = target;
            SetState(state);
        }

        public void SetMeleeCycleAttack(IHitBox hitBox)
        {
            var state = GetState<EnemyCycleMeleeAttackState>();
            state.HitBox = hitBox;
            SetState<EnemyCycleMeleeAttackState>();
        }

        public void SetMeleeAttack(IHitBox hitBox)
        {
            var state = GetState<EnemyMeleeAttackState>();
            state.HitBox = hitBox;
            SetState(state);
        }

        public void SetPatrol()
        {
            var state = GetState<EnemyPatrolState>();
            SetState(state);
        }
    }
}