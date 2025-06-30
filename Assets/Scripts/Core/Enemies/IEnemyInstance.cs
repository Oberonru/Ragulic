using Core.BaseComponents;
using Core.Configs.Enemies;

namespace Core.Enemies
{
    public interface IEnemyInstance
    {
        EnemyConfig Stats { get; }
        IHealthComponent HealthComponent { get; }
    }
}