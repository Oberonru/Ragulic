using Core.BaseComponents;
using Core.Configs.Player;
using UnityEngine;

namespace Core.Player
{
    public interface IPlayerInstance
    {
        Transform Transform { get; }
        PlayerConfig Stats { get; }
        IHealthComponent Health { get; }
    }
}