using System.StateMachineSystem;
using UnityEngine;

namespace Core.Enemies.States
{
    public class EnemyPatrolState : StateInstance<EnemyInstance>
    {
        public override void Enter()
        {
            Debug.Log("EnemyPatrolState");
            Patrol();
        }

        public override void Exit()
        {
            Owner.NavMesh.Stop();
        }

        private void Patrol()
        {
            //движение по случайным точкам, ...а как быть с SearchState, или это сделать компонентом, чтобы поиск
            // и луч были не зависимо от состояния врага?

            var rnd = Random.Range(0, Owner.EnemyData.Waypoints.Length);
            Owner.StateMachine.SetMeleeMoveToTarget(Owner.EnemyData.Waypoints[rnd]);
        }
    }
}