using System.Linq;
using System.StateMachineSystem;
using Core.Player;
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

                if (Vector3.Distance(Owner.Position, Target.position) > Owner.NavMesh.AI.SeeDistance)
                {
                    Owner.StateMachine.SetPatrol();
                }
            }
        }

        
    }
}