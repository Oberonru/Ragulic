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
        private bool _isSee;
        
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
            
            if (IsSeePlayer() && InFOV())
            {
                Owner.StateMachine.SetMeleeMoveToTarget(_player.Transform);
            }
        }

        private bool IsSeePlayer()
        {
            Physics.Raycast(Owner.Position, Owner.Transform.forward, out RaycastHit hit,
                Owner.NavMesh.AI.SeeDistance);
            
            Debug.Log(hit);

            if (hit.collider != null && hit.collider.TryGetComponent(out IPlayerInstance player))
            {
                _player = player;
                Owner.EnemyData.IsSee = true;
                return true;
            }
            
            Owner.EnemyData.IsSee = false;
            _player = null;
            
            return false;
        }

        private bool InFOV()
        {
            var direction = _player.Transform.position - Owner.Position;
            var angle = Vector3.Angle(direction, Owner.Transform.forward);
            
            return angle >= Owner.NavMesh.AI.MinFOV && angle <= Owner.NavMesh.AI.MaxFOV;
        }
    }
}