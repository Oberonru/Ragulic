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
        [SerializeField] private HeartUI _heart;
        private Transform _playerTransform;
        private float _detectedRadius;
        
        private void Start()
        {
            if (_heart is null) _heart = GetComponent<HeartUI>();
        }

        private void Update()
        {
            var distance = DistanceToTarget();
            
            _heart.Beat(distance, _detectedRadius);
        }

        public void SetData(Transform playerTransform, float detectedRadius)
        {
            _playerTransform = playerTransform;
            _detectedRadius = detectedRadius;
        }

        private float DistanceToTarget()
        {
            return Vector3.Distance(_playerTransform.position, _enemy.Transform.position);
        }
    }
}