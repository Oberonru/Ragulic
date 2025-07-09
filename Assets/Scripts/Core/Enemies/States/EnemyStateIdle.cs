using System.Interfaces;
using System.Linq;
using System.StateMachineSystem;
using Core.Player;
using UnityEngine;

namespace Core.Enemies.States
{
    public class EnemyStateIdle : StateInstance<EnemyInstance>, IFixedUpdate
    {
        private IPlayerInstance _player;
        
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
                Physics.SphereCastAll(Owner.Position,
                    Owner.NavMesh.AI.AgressiveRadius * Owner.NavMesh.AI.AgressiveMultiplayer,
                    Owner.transform.forward);

            var playerHit =
                colliders.FirstOrDefault((x) => x.collider.TryGetComponent(out IPlayerInstance playerInstance));

            if (playerHit.collider != null && playerHit.collider.TryGetComponent(out IPlayerInstance playerInstance))
            {
                Owner.StateMachine.SetMeleeMoveToTarget(playerHit.transform);
            }

            if (IsSeePlayer())
            {
                Owner.StateMachine.SetMeleeMoveToTarget(_player.Transform);
            }
        }

        private bool IsSeePlayer()
        {
            Physics.Raycast(Owner.Position, Owner.Transform.forward, out RaycastHit hit,
                Owner.NavMesh.AI.SeeDistance);

            if (hit.collider != null && hit.collider.TryGetComponent(out IPlayerInstance player))
            {
                _player = player;
                Owner.Stats.IsSee = true;
                return true;
            }
            
            Owner.Stats.IsSee = false;
            return false;
        }
    }
}