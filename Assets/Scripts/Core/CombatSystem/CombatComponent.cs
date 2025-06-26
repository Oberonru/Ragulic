using UnityEngine;

namespace Core.CombatSystem
{
    public abstract class CombatComponent : MonoBehaviour, ICombatComponent
    {
        public int Damage
        {
            get => _damage;
            protected set => _damage = value;
        }

        private int _damage;

        public void DefaultDamage(int damage)
        {
            _damage = damage;
        }

        public virtual void RandomDamage(int damage) {}
    }
}