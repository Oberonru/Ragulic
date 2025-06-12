using Core.BaseComponents;
using Core.Configs.Player;

namespace Core.Player
{
    public interface IPlayerInstance
    {
        PlayerConfig Stats { get; }
        IHealthComponent Health { get; }
    }
}