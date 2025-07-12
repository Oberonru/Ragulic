using Core.BaseComponents;
using Core.Configs.Enemies;
using Core.Enemies.CombatSystem;
using Core.Enemies.Components;
using Core.Enemies.Data;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Core.Enemies
{
    [RequireComponent(typeof(HealthComponent))]
    [RequireComponent(typeof(EnemyNavMesh))]
    [RequireComponent(typeof(EnemyStateMachine))]
    [RequireComponent(typeof(EnemyCombatComponent))]
    public class EnemyInstance : MonoBehaviour, IEnemyInstance
    {
        [SerializeField] private EnemyConfig _enemyStats;
        [SerializeField, ReadOnly] private HealthComponent _health;
        [SerializeField, ReadOnly] private EnemyNavMesh _navMesh;
        [SerializeField, ReadOnly] private EnemyStateMachine _stateMachine;
        [SerializeField, ReadOnly] private EnemyCombatComponent _enemyCombat;
        [SerializeField, ReadOnly] private RuntimeEnemyData _enemyData;

        public Vector3 Position => transform != null ? transform.position : Vector3.zero;
        public Transform Transform => transform;

        public IHealthComponent HealthComponent => _health;
        public EnemyNavMesh NavMesh => _navMesh;
        public EnemyConfig Stats => _enemyStats;
        public EnemyStateMachine StateMachine => _stateMachine;
        public EnemyCombatComponent EnemyCombatComponent => _enemyCombat;
        public RuntimeEnemyData EnemyData => _enemyData;

        private void Awake()
        {
            _health.MaxHealth = _enemyStats.Health;
            _health.CurrentHealth = _health.MaxHealth;
        }

        private void OnDrawGizmos()
        {
            if (_enemyData is null) return;
            
            Debug.DrawRay(Position, Transform.forward * NavMesh.AI.SeeDistance * NavMesh.AI.AgressiveMultiplayer,
                _enemyData.IsSee ? Color.green : Color.red);
            
            Debug.Log(StateMachine.GetActiveState() + " :current active state");
        }

        private void OnValidate()
        {
            if (_health is null) _health = GetComponent<HealthComponent>();
            if (_navMesh is null) _navMesh = GetComponent<EnemyNavMesh>();
            if (_enemyCombat is null) _enemyCombat = GetComponent<EnemyCombatComponent>();
            if (_enemyData is null) _enemyData = GetComponent<RuntimeEnemyData>();
        }
    }
}