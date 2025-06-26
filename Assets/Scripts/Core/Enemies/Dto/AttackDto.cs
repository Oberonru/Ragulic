using Core.CombatSystem;
using UnityEngine;

namespace Core.Enemies.Dto
{
    public class AttackDto : IAttackDto
    {
        public IHitBox HitBox { get; set; }
        public Collision Collision { get; set; }

        public void Initialize(IHitBox hitBox, Collision collision)
        {
            HitBox = hitBox;
            Collision = collision;
        }
    }
}