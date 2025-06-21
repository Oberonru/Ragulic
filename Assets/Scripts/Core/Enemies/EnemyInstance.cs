using Core.BaseComponents;
using Core.Configs.Enemies;
using Core.Enemies.CombatSystem;
using Core.Enemies.Components;
using Unity.Collections;
using UnityEngine;

namespace Core.Enemies
{
    [RequireComponent(typeof(HealthComponent))]
    [RequireComponent(typeof(EnemyNavMesh))]
    [RequireComponent(typeof(EnemyStateMachine))]
    [RequireComponent(typeof(EnemyCombatComponent))]
    public class EnemyInstance : MonoBehaviour
    {
        [SerializeField] private EnemyConfig _enemyStats;
        [SerializeField, ReadOnly] private HealthComponent _health;
        [SerializeField, ReadOnly] private EnemyNavMesh navMesh;
        [SerializeField, ReadOnly] private EnemyStateMachine _stateMachine;
        [SerializeField, ReadOnly] private EnemyCombatComponent _enemyCombat;

        public IHealthComponent HealthComponent => _health;
        public EnemyNavMesh NavMesh => navMesh;
        public EnemyConfig EnemyStats => _enemyStats;
        public EnemyStateMachine StateMachine => _stateMachine;

        public EnemyCombatComponent EnemyCombat => _enemyCombat;
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
            if (_enemyCombat is null) GetComponent<EnemyCombatComponent>();
        }
    }
}