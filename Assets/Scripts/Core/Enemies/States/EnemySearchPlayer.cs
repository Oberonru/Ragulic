using System.Interfaces;
using System.Linq;
using System.StateMachineSystem;
using Core.Player;
using UnityEngine;

namespace Core.Enemies.States
{
    public class EnemySearchPlayer : StateInstance<EnemyInstance>, IFixedUpdate
    {
        private IPlayerInstance _player;
        private bool _isSee;
        private float _aggrRadius;
        private float _maxSeedDistance;

        public override void Enter()
        {
            _aggrRadius = Owner.NavMesh.AI.AgressiveRadius * Owner.NavMesh.AI.AgressiveMultiplayer;
            _maxSeedDistance = Owner.NavMesh.AI.SeeDistance * Owner.NavMesh.AI.AgressiveMultiplayer;
        }

        public override void Exit()
        {
            _player = null;
            Owner.EnemyData.IsSee = false;
            Owner.NavMesh.Stop();
        }

        public void FixedUpdate()
        {
            if (AlwaysSearch())
            {
                Owner.StateMachine.SetMeleeMoveToTarget(_player.Transform);
            }

            else if (IsSeePlayer() && InFOV())
            {
                Owner.StateMachine.SetMeleeMoveToTarget(_player.Transform);
            }

            else 
            {
                Owner.StateMachine.SetMeleeMoveToTarget(Owner.EnemyData.Waypoints);
                //Owner.StateMachine.SetPatrol();
            }
        }

        private bool AlwaysSearch()
        {
            var colliders = Physics.OverlapSphere(Owner.Position, _aggrRadius);
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
            Physics.Raycast(Owner.Position, Owner.Transform.forward, out RaycastHit hit, _maxSeedDistance);

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

            //DrawFovAnle(angle);

            return angle >= Owner.NavMesh.AI.MinFOV && angle <= Owner.NavMesh.AI.MaxFOV;
        }

        private void Patrol()
        {
            //WayPoints[] массив точек патрулирования
            //Берем случайную точку и запускаем состояние ходьбы
        }

        private void DrawFovAnle(float angle)
        {
            var radius = 10f;

            Vector3 origin = Owner.Transform.position;
            Vector3 forward = Owner.Transform.forward;

            // Граница левого края FOV
            Vector3 leftBoundary = Quaternion.Euler(0, -80, 0) * forward * radius;
            // Граница правого края FOV
            Vector3 rightBoundary = Quaternion.Euler(0, 90, 0) * forward * radius;

            Gizmos.color = Color.green;
            Gizmos.DrawRay(origin, leftBoundary);
            Gizmos.DrawRay(origin, rightBoundary);

            // Для большей наглядности — дополнительные лучи внутри сектора
            int rays = 10;
            for (int i = 0; i <= rays; i++)
            {
                var lerp = Mathf.Lerp(-80, 80, i / (float)rays);
                Vector3 dir = Quaternion.Euler(0, lerp, 0) * forward * radius;
                Gizmos.color = Color.yellow;
                Gizmos.DrawRay(origin, dir);
            }
        }
    }

    // var colliders =
    //     Physics.SphereCastAll(Owner.Position, _aggrRadius, Owner.transform.forward, _aggrRadius);
    //
    // var playerHit =
    //     colliders.FirstOrDefault((x) => x.collider.TryGetComponent(out IPlayerInstance playerInstance));
    //
    // if (playerHit.collider != null &&
    //     playerHit.collider.TryGetComponent(out IPlayerInstance playerInstance) || (IsSeePlayer() && InFOV()))
    // {
    //     Owner.StateMachine.SetMeleeMoveToTarget(playerHit.transform);
    // }

    // if (IsSeePlayer() && InFOV())
    // {
    //     Owner.StateMachine.SetMeleeMoveToTarget(_player.Transform);
    // }
}