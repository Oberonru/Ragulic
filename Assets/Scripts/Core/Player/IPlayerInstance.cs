using Core.BaseComponents;
using Core.Configs.Player;
using Core.Player.CombatSystem;
using Core.Player.Components;
using Core.Player.StateMachine;
using UnityEngine;

namespace Core.Player
{
    public interface IPlayerInstance
    {
        Transform Transform { get; }
        PlayerConfig Stats { get; }
        IHealthComponent Health { get; }
        SimplePlayerController PlayerController { get; }
        PlayerCombatComponent CombatComponent { get; }
        PlayerStateMachine StateMachine { get; }
        InventoryPlayerHandler InventoryHandler { get; }
    }
}