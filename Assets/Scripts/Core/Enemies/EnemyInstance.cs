using Core.BaseComponents;
using Core.Enemies.Components;
using Core.Enemies.SO;
using UnityEngine;

namespace Core.Enemies
{
    public class EnemyInstance : MonoBehaviour
    {
        [SerializeField] private EnemyConfig _enemyStats;
        [SerializeField] private HealthComponent _health;
        [SerializeField] private EnemyNavMesh _enemyNavMesh;

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