using Core.BaseComponents;
using Unity.Collections;
using UnityEngine;

namespace Core.CombatSystem
{
    public class HitBox : MonoBehaviour, IHitBox
    {
        public IHealthComponent HealthComponent => _healthComponent;
        [SerializeField, ReadOnly] private HealthComponent _healthComponent;
        public Rigidbody Rigidbody => _rigidbody;
        [SerializeField] private Rigidbody _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }
        
        private void OnValidate()
        {
            if (_healthComponent is null) _healthComponent = GetComponent<HealthComponent>();
        }
    }
}