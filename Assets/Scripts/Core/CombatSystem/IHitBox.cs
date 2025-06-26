using Core.BaseComponents;
using UnityEngine;

namespace Core.CombatSystem
{
    public interface IHitBox
    {
        IHealthComponent HealthComponent { get; }

        Rigidbody Rigidbody { get; }
    }
}