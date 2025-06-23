using Core.CombatSystem;
using UnityEngine;

namespace Core.Player.CombatSystem
{
    public interface IPlayerHitBox : IHitBox
    {
        Rigidbody Rigidbody { get; }
    }
}