using Core.BaseComponents;
using Core.Player.SO;
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