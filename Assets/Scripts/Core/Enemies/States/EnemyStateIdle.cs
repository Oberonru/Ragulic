using System.Interfaces;
using System.Linq;
using System.StateMachineSystem;
using Core.Player;
using UnityEngine;

namespace Core.Enemies.States
{
    public class EnemyStateIdle : StateInstance<EnemyInstance>, IFixedUpdate
    {
        public override void Enter()
        {
            Owner.NavMesh.Stop();
        }

        public override void Exit()
        {
        }

        public void FixedUpdate()
        {
            var colliders =
                Physics.SphereCastAll(Owner.Position, Owner.NavMesh.AI.AgressiveRadius * Owner.NavMesh.AI.AgressiveMultiplayer,
                    Owner.transform.forward);

            var playerHit =
                colliders.FirstOrDefault((x) => x.collider.TryGetComponent(out IPlayerInstance playerInstance));

            if (playerHit.collider != null && playerHit.collider.TryGetComponent(out IPlayerInstance playerInstance))
            {
                Owner.StateMachine.SetMeleeMoveToTarget(playerHit.transform);
            }

            if (IsSeePlayer())
            {
                Debug.Log("See player");
            }
        }

        private bool IsSeePlayer()
        {
            bool hit = Physics.Raycast(Owner.Position, Owner.Transform.forward, out RaycastHit raycast, Owner.NavMesh.AI.SeeDistance);
            Debug.DrawRay(Owner.Position, raycast.point - Owner.Position, Color.red);
            
            return hit;
        }
    }
}