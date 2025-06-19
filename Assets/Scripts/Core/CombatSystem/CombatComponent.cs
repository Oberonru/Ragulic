using UnityEngine;

namespace Core.CombatSystem
{
    public class CombatComponent : MonoBehaviour, ICombatComponent
    {
        public int Damage => _damage;
        private int _damage;
        
        public void SetDamage(int damage)
        {
            _damage = damage;
        }
    }
}