using System.Interfaces;
using System.StateMachineSystem;
using UnityEngine;

namespace Core.Enemies.States
{
    public class EnemyMeleeMoveState : StateInstance<EnemyInstance>, IFixedUpdate
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

        public void FixedUpdate()
        {
            Move();
        }

        private void Move()
        {
            if (Target != null)
            {
                Owner.NavMesh.MoveToTarget(Target.position);
                if (Vector3.Distance(Owner.Position, Target.position) > Owner.NavMesh.AI.AgressiveRadius)
                {
                    Owner.NavMesh.Stop();
                }
            }
            else
            {
                Owner.NavMesh.Stop();
            }
        }
    }
}