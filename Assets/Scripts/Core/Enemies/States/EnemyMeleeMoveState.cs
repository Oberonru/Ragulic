using System.Linq;
using System.StateMachineSystem;
using Core.Player;
using UnityEngine;

namespace Core.Enemies.States
{
    public class EnemyMeleeMoveState : StateInstance<EnemyInstance>, IUpdate
    {
        public Transform Target;
        private IPlayerInstance _player;

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


                if (Vector3.Distance(Owner.NavMesh.transform.position, Target.position) > Owner.NavMesh.AI.SeeDistance)
                {
                    Owner.StateMachine.SetPatrol();
                }
            }
            
            else if (PlayerIsDetected())
            {
                Owner.NavMesh.MoveToTarget(_player.Transform.position);
            }
        }

        public bool PlayerIsDetected()
        {
            return AlwaysSearch() || (IsSeePlayer() && InFOV());
        }

        private bool AlwaysSearch()
        {
            var colliders = Physics.OverlapSphere(Owner.Position,
                Owner.NavMesh.AI.AgressiveRadius * Owner.NavMesh.AI.AgressiveMultiplayer);
            var collider = colliders.FirstOrDefault(collider => collider.TryGetComponent<IPlayerInstance>(out var _));

            if (collider != null && collider.TryGetComponent<IPlayerInstance>(out var player))
            {
                _player = player;
                return true;
            }

            _player = null;
            return false;
        }

        private bool IsSeePlayer()
        {
            Physics.Raycast(Owner.Position, Owner.Transform.forward, out RaycastHit hit, Owner.NavMesh.AI.SeeDistance);

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
            if (_player == null) return false;

            var direction = _player.Transform.position - Owner.Position;
            var angle = Vector3.Angle(direction, Owner.Transform.forward);

            return angle >= Owner.NavMesh.AI.MinFOV && angle <= Owner.NavMesh.AI.MaxFOV;
        }
    }
}