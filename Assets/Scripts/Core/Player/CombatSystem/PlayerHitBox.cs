using Core.BaseComponents;
using UnityEngine;

namespace Core.Player.CombatSystem
{
    public class PlayerHitBox : MonoBehaviour,  IPlayerHitBox
    {
        public IHealthComponent HealthComponent => _healthComponent;
        [SerializeField] private HealthComponent _healthComponent;

        private void OnValidate()
        {
            if (_healthComponent is null) _healthComponent = GetComponent<HealthComponent>();
        }
    }
}