using System.StateMachineSystem;
using UnityEngine;

namespace Core.Enemies.States
{
    public class EnemyMeleeMoveState : StateInstance<EnemyInstance>, IUpdate
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
            if (Target != null)
            {
                Owner.NavMesh.MoveToTarget(Target.position);

                //     if (!Owner.EnemyData.IsSee && Vector3.Distance(Owner.Position, Target.position) > AggroZone())
                //     {
                //         Owner.StateMachine.SetIdle();
                //     }
                // }
                // else
                // {
                //     Owner.StateMachine.SetIdle();
            }

            else if (Owner.EnemyData.IsSee)
            {
                Owner.StateMachine.SetSearchPlayer();
            }
        }

        private float AggroZone()
        {
            return Owner.NavMesh.AI.AgressiveRadius * Owner.NavMesh.AI.AgressiveMultiplayer;
        }
    }
}