using System;
using Core.BaseComponents;
using Core.CombatSystem;
using Core.Configs.Player;
using Core.Player.CombatSystem;
using Core.Player.Components;
using Core.Player.StateMachine;
using Sirenix.OdinInspector;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Core.Player
{
    [RequireComponent(typeof(HealthComponent))]
    [RequireComponent(typeof(PlayerController))]
    [RequireComponent(typeof(PlayerCombatComponent))]
    [RequireComponent(typeof(PlayerStateMachine))]
    [RequireComponent(typeof(InventoryPlayerHandler))]
    [RequireComponent(typeof(TriggerHitBoxDetector))]
    [RequireComponent(typeof(PlayerAnimator))]
    public class PlayerInstance : MonoBehaviour, IPlayerInstance
    {
        [Inject] private PlayerConfig _playerStats;
        [SerializeField, ReadOnly] private HealthComponent _health;
        [SerializeField, ReadOnly] private PlayerController playerController;
        [SerializeField, ReadOnly] private PlayerCombatComponent _combatComponent;
        [SerializeField, ReadOnly] private PlayerStateMachine _stateMachine;
        [SerializeField, ReadOnly] private InventoryPlayerHandler _inventory;
        [SerializeField, ReadOnly] private TriggerHitBoxDetector _triggerHitBoxDetector;
        [SerializeField, ReadOnly] private PlayerAnimator _animator;

        private CinemachineInputAxisController _inputAxisController;

        public Transform Transform => transform;
        public PlayerConfig Stats => _playerStats;
        public IHealthComponent Health => _health;
        public PlayerController PlayerController => playerController;
        public PlayerCombatComponent CombatComponent => _combatComponent;
        public PlayerStateMachine StateMachine => _stateMachine;
        public InventoryPlayerHandler InventoryHandler => _inventory;
        public TriggerHitBoxDetector TriggerHitBoxDetector => _triggerHitBoxDetector;
        public PlayerAnimator Animator => _animator;

        private void Awake()
        {
            if (_playerStats is null)
                throw new NullReferenceException($"PlayerConfig is not set on player {gameObject.name}");
            _health.MaxHealth = Stats.Health;
            _health.CurrentHealth = _health.MaxHealth;
        }

        private void Start()
        {
            _inputAxisController = GetComponent<CinemachineInputAxisController>();
            if (_inputAxisController != null)
            {
                _inputAxisController.enabled = false;
                _inputAxisController.enabled = true;
            }
        }

        private void OnValidate()
        {
            if (_health is null) _health = GetComponent<HealthComponent>();
            if (playerController is null) playerController = GetComponent<PlayerController>();
            if (_combatComponent is null) _combatComponent = GetComponent<PlayerCombatComponent>();
            if (_stateMachine is null) _stateMachine = GetComponent<PlayerStateMachine>();
            if (_inventory is null) _inventory = GetComponent<InventoryPlayerHandler>();
            if (_triggerHitBoxDetector is null) _triggerHitBoxDetector = GetComponent<TriggerHitBoxDetector>();
            if (_animator is null) _animator = GetComponent<PlayerAnimator>();
        }
    }
}