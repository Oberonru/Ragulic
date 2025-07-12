using System.StateMachineSystem;
using UnityEngine;

namespace Core.Enemies.States
{
    public class EnemyPatrolState : StateInstance<EnemyInstance>, IUpdate
    {
        public override void Enter()
        {
            Patrol();
        }

        public override void Exit()
        {
            Owner.NavMesh.Stop();
        }

        public void Update()
        {
            Patrol();
        }

        private void Patrol()
        {
            var rnd = Random.Range(0, Owner.EnemyData.Waypoints.Length);
            Owner.NavMesh.MoveToTarget(Owner.EnemyData.Waypoints[rnd].position);
        }
    }
}