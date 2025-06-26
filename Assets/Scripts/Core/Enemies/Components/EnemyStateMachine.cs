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

        public void SetMeleeCycleAttack(IHitBox hitBox)
        {
            var state = GetState<EnemyCycleMeleeAttackState>();
            state.HitBox = hitBox;
            SetState<EnemyCycleMeleeAttackState>();
        }

        public void SetMeleeAttack(IHitBox hitBox, Collision collision)
        {
            var state = GetState<EnemyMeleeAttackState>();
            state.HitBox = hitBox;
            state.Collision = collision;
            SetState(state);
        }
    }
}