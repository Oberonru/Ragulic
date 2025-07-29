using Core.BaseComponents;
using Core.CombatSystem;
using UnityEngine;

namespace Core.Player.CombatSystem
{
    public interface IPlayerHitBox : IHitBox, IDisableComponent
    {
        Rigidbody Rigidbody { get; }
    }
}