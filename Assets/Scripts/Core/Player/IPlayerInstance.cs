using Core.BaseComponents;
using Core.Player.SO;

namespace Core.Player
{
    public interface IPlayerInstance
    {
        PlayerConfig Stats { get; }
        IHealthComponent Health { get; }
    }
}