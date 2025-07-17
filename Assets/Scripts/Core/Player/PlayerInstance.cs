using System;
using Core.BaseComponents;
using Core.Configs.Player;
using Core.Player.CombatSystem;
using Core.Player.Components;
using Core.Player.StateMachine;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Core.Player
{
    [RequireComponent(typeof(HealthComponent))]
    [RequireComponent(typeof(PlayerController))]
    [RequireComponent(typeof(PlayerCombatComponent))]
    [RequireComponent(typeof(PlayerStateMachine))]
    [RequireComponent(typeof(InventoryPlayerHandler))]
    public class PlayerInstance : MonoBehaviour, IPlayerInstance
    {
        [Inject] private PlayerConfig _playerStats;
        [SerializeField, ReadOnly] private HealthComponent _health;
        [SerializeField, ReadOnly] private PlayerController _simplePlayerController;
        [SerializeField, ReadOnly] private PlayerCombatComponent _combatComponent;
        [SerializeField, ReadOnly] private PlayerStateMachine _stateMachine;
        [SerializeField, ReadOnly] private InventoryPlayerHandler _inventory;

        public Transform Transform => transform;
        public PlayerConfig Stats => _playerStats;
        public IHealthComponent Health => _health;
        public PlayerController PlayerController => _simplePlayerController;
        public PlayerCombatComponent CombatComponent => _combatComponent;
        public PlayerStateMachine StateMachine => _stateMachine;
        public InventoryPlayerHandler InventoryHandler => _inventory;

        private void Awake()
        {
            if (_playerStats is null)
                throw new NullReferenceException($"PlayerConfig is not set on player {gameObject.name}");
            _health.MaxHealth = Stats.Health;
            _health.CurrentHealth = _health.MaxHealth;
        }

        private void OnValidate()
        {
            if (_health is null) _health = GetComponent<HealthComponent>();
            if (_simplePlayerController is null) _simplePlayerController = GetComponent<PlayerController>();
            if (_combatComponent is null) _combatComponent = GetComponent<PlayerCombatComponent>();
            if (_stateMachine is null) _stateMachine = GetComponent<PlayerStateMachine>();
            if (_inventory is null) _inventory = GetComponent<InventoryPlayerHandler>();
        }
    }
}