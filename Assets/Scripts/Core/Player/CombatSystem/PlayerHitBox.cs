using Core.BaseComponents;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Core.Player.CombatSystem
{
    public class PlayerHitBox : MonoBehaviour,  IPlayerHitBox
    {
        public Rigidbody Rigidbody => _rigidbody;
        [SerializeField] private Rigidbody _rigidbody;
        
        public IHealthComponent HealthComponent => _healthComponent;
        [SerializeField, ReadOnly] private HealthComponent _healthComponent;

        private void OnValidate()
        {
            if (_healthComponent is null) _healthComponent = GetComponent<HealthComponent>();
            if (_rigidbody is null) _rigidbody = GetComponent<Rigidbody>();
        }
    }
}