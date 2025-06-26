using Core.BaseComponents;
using Core.Configs.Enemies;
using Core.Enemies.CombatSystem;
using Core.Enemies.Components;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Core.Enemies
{
    [RequireComponent(typeof(HealthComponent))]
    [RequireComponent(typeof(EnemyNavMesh))]
    [RequireComponent(typeof(EnemyStateMachine))]
    //[RequireComponent(typeof(EnemyCombatComponent))]
    public class EnemyInstance : MonoBehaviour
    {
        [SerializeField] private EnemyConfig _enemyStats;
        [SerializeField, ReadOnly] private HealthComponent _health;
        [SerializeField, ReadOnly] private EnemyNavMesh _navMesh;
        [SerializeField, ReadOnly] private EnemyStateMachine _stateMachine;
        [SerializeField, ReadOnly] private EnemyCombatComponent _enemyCombat;

        public IHealthComponent HealthComponent => _health;
        public EnemyNavMesh NavMesh => _navMesh;
        public EnemyConfig EnemyStats => _enemyStats;
        public EnemyStateMachine StateMachine => _stateMachine;
        public EnemyCombatComponent EnemyCombatComponent => _enemyCombat;

        public Vector3 Position => transform != null ? transform.position : Vector3.zero;

        private void Awake()
        {
            _health.MaxHealth = _enemyStats.Health;
            _health.CurrentHealth = _health.MaxHealth;
        }

        private void OnValidate()
        {
            if (_health is null) _health = GetComponent<HealthComponent>();
            if (_navMesh is null) _navMesh = GetComponent<EnemyNavMesh>();
            if (_enemyCombat is null) _enemyCombat = GetComponent<EnemyCombatComponent>();
        }
    }
}