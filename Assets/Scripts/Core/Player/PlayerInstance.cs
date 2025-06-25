using System;
using Core.BaseComponents;
using Core.Player.Components;
using Core.Player.SO;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Core.Player
{
    [RequireComponent(typeof(IHealthComponent))]
    public class PlayerInstance : MonoBehaviour, IPlayerInstance
    {
        [Inject] private PlayerConfig _playerStats;
        [SerializeField, ReadOnly] private HealthComponent _health;
        [SerializeField, ReadOnly] private RigidbodyPlayerMovement _movement;

        public Transform Transform => transform;
        public PlayerConfig Stats => _playerStats;
        public IHealthComponent Health => _health;

        private void Awake()
        {
            if (_playerStats is null)
                throw new NullReferenceException($"PlayerConfig is not seted on player {gameObject.name}");
            _health.MaxHealth = Stats.Health;
            _health.CurrentHealth = _health.MaxHealth;
        }

        private void OnValidate()
        {
            //if (_movement is null) GetComponent<PlayerMovement>();
            if (_health is null) _health = GetComponent<HealthComponent>();
            if (_movement is null) _movement = GetComponent<RigidbodyPlayerMovement>();
        }
    }
}