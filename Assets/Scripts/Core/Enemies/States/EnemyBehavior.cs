using System.Linq;
using Core.Player;
using UnityEngine;

namespace Core.Enemies.States
{
    public class EnemyBehavior : MonoBehaviour
    {
        [SerializeField] private EnemyInstance _enemy;
        public  IPlayerInstance Player;
        private float _aggrRadius;
        private float _maxSeedDistance;

        private void OnEnable()
        {
            _aggrRadius = _enemy.NavMesh.AI.AgressiveRadius * _enemy.NavMesh.AI.AgressiveMultiplayer;
            _maxSeedDistance = _enemy.NavMesh.AI.SeeDistance * _enemy.NavMesh.AI.AgressiveMultiplayer;
        }

        private void Update()
        {
            if (PlayerIsDetected())
            {
                _enemy.StateMachine.SetMeleeMoveToTarget(Player.Transform);
            }
            
            else
            {
                _enemy.StateMachine.SetPatrol();
            }
        }

        public bool PlayerIsDetected()
        {
            return AlwaysSearch() || (IsSeePlayer() && InFOV());
        }

        private bool AlwaysSearch()
        {
            var colliders = Physics.OverlapSphere(_enemy.Position, _aggrRadius);
            var collider = colliders.FirstOrDefault(collider => collider.TryGetComponent<IPlayerInstance>(out var _));

            if (collider != null && collider.TryGetComponent<IPlayerInstance>(out var player))
            {
                Player = player;
                return true;
            }

            Player = null;
            return false;
        }

        private bool IsSeePlayer()
        {
            Physics.Raycast(_enemy.Position, _enemy.Transform.forward, out RaycastHit hit, _maxSeedDistance);

            if (hit.collider != null && hit.collider.TryGetComponent(out IPlayerInstance player))
            {
                Player = player;
                _enemy.EnemyData.IsSee = true;
                return true;
            }

            _enemy.EnemyData.IsSee = false;
            Player = null;

            return false;
        }

        private bool InFOV()
        {
            var direction = Player.Transform.position - _enemy.Position;
            var angle = Vector3.Angle(direction, _enemy.Transform.forward);

            //DrawFovAnle(angle);

            return angle >= _enemy.NavMesh.AI.MinFOV && angle <= _enemy.NavMesh.AI.MaxFOV;
        }
    }
}