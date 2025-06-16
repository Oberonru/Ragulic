using Core.BaseComponents;
using Core.Configs.Enemies;
using Core.Enemies.Components;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace Core.Enemies
{
    public class EnemyInstance : MonoBehaviour
    {
        [SerializeField] private EnemyConfig _enemyStats;
        [SerializeField, ReadOnly] private HealthComponent _health;
        [SerializeField, ReadOnly] private EnemyNavMesh navMesh;
        [SerializeField, ReadOnly] private EnemyStateMachine _stateMachine;

        public HealthComponent HealthComponent => _health;
        public EnemyNavMesh NavMesh => navMesh;
        public EnemyConfig EnemyStats => _enemyStats;
        public EnemyStateMachine StateMachine => _stateMachine;
        public Vector3 Position => transform != null ? transform.position : Vector3.zero;
        
        private void Awake()
        {
            _health.MaxHealth = _enemyStats.Health;
            _health.CurrentHealth = _health.MaxHealth;
        }

        private void OnValidate()
        {
            if (_health is null) GetComponent<HealthComponent>();
            if (navMesh is null) GetComponent<EnemyNavMesh>();
        }
    }
}