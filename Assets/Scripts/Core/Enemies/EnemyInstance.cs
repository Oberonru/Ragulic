using Core.BaseComponents;
using Core.Enemies.Components;
using Core.Enemies.SO;
using Unity.Collections;
using UnityEngine;

namespace Core.Enemies
{
    public class EnemyInstance : MonoBehaviour
    {
        [SerializeField] private EnemyConfig _enemyStats;
        [SerializeField, ReadOnly] private HealthComponent _health;
        [SerializeField, ReadOnly] private EnemyNavMesh _enemyNavMesh;

        public EnemyNavMesh EnemyNavMesh => _enemyNavMesh;
        
        public EnemyConfig EnemyStats => _enemyStats;
        
        private void Awake()
        {
            _health.MaxHealth = _enemyStats.Health;
        }

        private void OnValidate()
        {
            if (_health is null) GetComponent<HealthComponent>();
        }
    }
}