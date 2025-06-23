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
        [FormerlySerializedAs("_enemyCombat")] [SerializeField, ReadOnly] private EnemyTriggerCombatComponent enemyTriggerCombat;
        [FormerlySerializedAs("_enemyColliderCombat")] [SerializeField, ReadOnly] private EnemyCollisionCombatComponent enemyCollisionCombat;

        public IHealthComponent HealthComponent => _health;
        public EnemyNavMesh NavMesh => _navMesh;
        public EnemyConfig EnemyStats => _enemyStats;
        public EnemyStateMachine StateMachine => _stateMachine;

        public EnemyTriggerCombatComponent EnemyTriggerCombat => enemyTriggerCombat;
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
            if (enemyTriggerCombat is null) enemyTriggerCombat = GetComponent<EnemyTriggerCombatComponent>();
            if (enemyCollisionCombat is null) enemyCollisionCombat = GetComponent<EnemyCollisionCombatComponent>();
        }
    }
}