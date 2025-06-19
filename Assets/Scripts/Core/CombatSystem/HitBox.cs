using Core.BaseComponents;
using Unity.Collections;
using UnityEngine;

namespace Core.CombatSystem
{
    public class HitBox : MonoBehaviour, IHitBox
    {
        public IHealthComponent HealthComponent => _healthComponent;
        [SerializeField, ReadOnly] private HealthComponent _healthComponent;

        private void OnValidate()
        {
            if (_healthComponent is null) _healthComponent = GetComponent<HealthComponent>();
        }
    }
}