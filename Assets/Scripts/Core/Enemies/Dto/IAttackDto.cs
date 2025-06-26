using Core.CombatSystem;
using UnityEngine;

namespace Core.Enemies.Dto
{
    public interface IAttackDto
    {
        IHitBox HitBox { get; set; }
        Collision Collision { get; set;  }
    }
}