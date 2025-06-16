using System.Interfaces;
using System.Linq;
using System.StateMachineSystem;
using Core.Player;
using UnityEngine;

namespace Core.Enemies.States
{
    public class EnemyStateIdle : StateInstance<EnemyInstance>, IFixedUpdate, IDrawGizmos
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
                Physics.SphereCastAll(Owner.Position, Owner.NavMesh.AI.AgressiveRadius,
                    Owner.transform.forward);

            var playerHit =
                colliders.FirstOrDefault((x) => x.collider.TryGetComponent(out IPlayerInstance playerInstance));

            if (playerHit.collider != null && playerHit.collider.TryGetComponent(out IPlayerInstance playerInstance))
            {
                Owner.NavMesh.MoveToTarget(playerHit.transform.position);
            }
        }

        public void DrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(Owner.Position, Owner.NavMesh.AI.AgressiveRadius);
        }
    }
}