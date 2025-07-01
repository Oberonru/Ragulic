using Core.Enemies;
using Core.Player.Components;
using UnityEngine;
using Zenject;

namespace System.UI.Heart
{
    public class HeartBeat : MonoBehaviour, IHeartBeatComponent
    {
        [Inject] private IEnemyInstance _enemy;
        public HeartUI Heart => _heart;
        private Transform _playerTransform;
        private float _detectedRadius;

        [SerializeField] private HeartUI _heart;

        private void Start()
        {
            if (_heart is null) _heart = GetComponent<HeartUI>();
        }

        private void FixedUpdate()
        {
            var distance = DistanceToTarget();
            
            _heart.Beat(distance, _detectedRadius);
        }

        public void SetData(Transform transform, float detectedRadius)
        {
            _playerTransform = transform;
            _detectedRadius = detectedRadius;
        }

        private float DistanceToTarget()
        {
            return Vector3.Distance(_playerTransform.position, _enemy.Transform.position);
        }
    }
}