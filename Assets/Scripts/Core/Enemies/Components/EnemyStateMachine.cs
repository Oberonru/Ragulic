using System.StateMachineSystem;
using Core.Enemies.States;
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

        public void SetMeleeAttack(Transform target)
        {
            var state = GetState<EnemyMeleeAttackState>();
            state.Target = target;
            SetState<EnemyMeleeAttackState>();
        }
    }
}