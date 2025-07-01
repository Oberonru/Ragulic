using System;
using Core.BaseComponents;
using Core.Configs.Player;
using Core.Player.CombatSystem;
using Core.Player.Components;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Core.Player
{
    [RequireComponent(typeof(HealthComponent))]
    [RequireComponent(typeof(PlayerMovement))]
    [RequireComponent(typeof(PlayerCombatComponent))]
    public class PlayerInstance : MonoBehaviour, IPlayerInstance
    {
        [Inject] private PlayerConfig _playerStats;
        [SerializeField, ReadOnly] private HealthComponent _health;
        [SerializeField, ReadOnly] private PlayerMovement _movement;
        [SerializeField, ReadOnly] private PlayerCombatComponent _combatComponent;

        public Transform Transform => transform;
        public PlayerConfig Stats => _playerStats;
        public IHealthComponent Health => _health;
        public PlayerCombatComponent CombatComponent => _combatComponent;

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
            if (_movement is null) _movement = GetComponent<PlayerMovement>();
            if (_combatComponent is null) _combatComponent = GetComponent<PlayerCombatComponent>();
        }
    }
}