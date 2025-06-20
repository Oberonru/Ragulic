using Core.BaseComponents;
using Core.CombatSystem;
using UnityEngine;

namespace Core.Player.CombatSystem
{
    public class PlayerHitBox :HitBox, IPlayerHitBox
    {
        public IHealthComponent HealthComponent => _healthComponent;
        [SerializeField] private HealthComponent _healthComponent;
    }
}