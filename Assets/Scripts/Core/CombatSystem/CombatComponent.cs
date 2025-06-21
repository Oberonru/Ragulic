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

        public void SetDefaultDamage(int damage)
        {
            _damage = damage;
        }

        public abstract void SetRandomDamage(int damage);
    }
}