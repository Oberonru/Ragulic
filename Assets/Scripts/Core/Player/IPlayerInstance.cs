using Core.BaseComponents;
using Core.Configs.Player;
using Core.Player.CombatSystem;
using UnityEngine;

namespace Core.Player
{
    public interface IPlayerInstance
    {
        Transform Transform { get; }
        PlayerConfig Stats { get; }
        IHealthComponent Health { get; }
        PlayerCombatComponent CombatComponent { get; }
    }
}