using System.Interfaces;
using System.StateMachineSystem;
using UnityEngine;

namespace Core.Enemies.States
{
    public class EnemyMeleeMoveState : StateInstance<EnemyInstance>, IUpdate, IDrawGizmos
    {
        public Transform Target;

        public override void Enter()
        {
            Move();
        }

        public override void Exit()
        {
            Owner.NavMesh.Stop();
        }

        public void Update()
        {
            Move();
        }

        private void Move()
        {
            if (Target != null || Owner.Stats.IsSee)
            {
                Owner.NavMesh.MoveToTarget(Target.position);
                
                if (!Owner.Stats.IsSee && Vector3.Distance(Owner.Position, Target.position) > AggroZone())
                {
                    Owner.StateMachine.SetIdle();
                }
            }
            else
            {
                Owner.StateMachine.SetIdle();
            }
        }

        private float AggroZone()
        {
            return Owner.NavMesh.AI.AgressiveRadius * Owner.NavMesh.AI.AgressiveMultiplayer;
        }
        
        public void DrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(Owner.Position, Owner.NavMesh.AI.AgressiveRadius * Owner.NavMesh.AI.AgressiveMultiplayer);
        }
    }
}