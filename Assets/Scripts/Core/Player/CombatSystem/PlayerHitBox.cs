using Core.BaseComponents;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Core.Player.CombatSystem
{
    public class PlayerHitBox : MonoBehaviour,  IPlayerHitBox
    {
        public Rigidbody Rigidbody => _rigidbody;
        [SerializeField, ReadOnly] private Rigidbody _rigidbody;
        
        public IHealthComponent HealthComponent => _healthComponent;
        [SerializeField] private HealthComponent _healthComponent;

        private void OnValidate()
        {
            if (_healthComponent is null) _healthComponent = GetComponentInParent<HealthComponent>();
            if (_rigidbody is null) _rigidbody = GetComponent<Rigidbody>();
        }

        public void Disable()
        {
            if (_rigidbody) _rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        }
    }
}