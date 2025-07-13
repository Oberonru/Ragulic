using System.Linq;
using Core.Player;
using UnityEngine;

namespace Core.Enemies.Components
{
    public class RangeDetector : MonoBehaviour
    {
        public PlayerInstance Player;
        [SerializeField] private EnemyInstance _enemy;

        private float _detectionRadius;

        private void OnEnable()
        {
            _detectionRadius = _enemy.NavMesh.AI.DetectionRadius * _enemy.NavMesh.AI.AgressiveMultiplayer;
        }

        public IPlayerInstance UpdateDetector()
        {
            var colliders = Physics.OverlapSphere(transform.position,
                _detectionRadius);

            var result =
                colliders.FirstOrDefault(collider => collider.TryGetComponent<IPlayerInstance>(out var player));

            if (colliders != null && result.TryGetComponent<IPlayerInstance>(out var player))
            {
                return player;
            }

            return null;
        }

        private void OnDrawGizmos()
        {
            if (_enemy is null) return;

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _detectionRadius);
        }
    }
}