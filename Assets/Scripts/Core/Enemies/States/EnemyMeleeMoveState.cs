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
                var distance = Vector3.Distance(Owner.NavMesh.transform.position, Target.position);
                
                Debug.Log(distance + " distance");
                if (distance > 15)
                {
                    Debug.Log("Idle");
                    Owner.StateMachine.SetPatrol();
                }
            }
        }
    }
}