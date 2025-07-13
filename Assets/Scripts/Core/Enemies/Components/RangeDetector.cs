using Sirenix.OdinInspector;
using UnityEngine;

namespace Core.Enemies.Components
{
    public class RangeDetector : MonoBehaviour
    {
        public GameObject DetectedTarget;
        public LayerMask DetectionMask;
        [SerializeField, ReadOnly] private EnemyInstance _enemy;

        private float _detectionRadius;

        private void OnEnable()
        {
            _detectionRadius = _enemy.NavMesh.AI.DetectionRadius * _enemy.NavMesh.AI.AgressiveMultiplayer;
        }

        private void OnValidate()
        {
            if (_enemy is null) _enemy = GetComponent<EnemyInstance>();
        }

        public GameObject UpdateDetector()
        {
            var colliders = Physics.OverlapSphere(transform.position,
                _detectionRadius, DetectionMask);

            DetectedTarget = colliders.Length > 0 ? colliders[0].gameObject : null;

            return DetectedTarget;
        }

        private void OnDrawGizmos()
        {
            if (_enemy is null) return;

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _detectionRadius);
        }
    }
}