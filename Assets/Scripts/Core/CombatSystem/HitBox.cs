using Core.BaseComponents;
using UnityEngine;

namespace Core.CombatSystem
{
    public class HitBox : MonoBehaviour, IHitBox
    {
        public IHealthComponent HealthComponent => _healthComponent;
        [SerializeField] private HealthComponent _healthComponent;
    }
}