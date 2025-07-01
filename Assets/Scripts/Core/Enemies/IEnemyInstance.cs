using Core.BaseComponents;
using Core.Configs.Enemies;
using UnityEngine;

namespace Core.Enemies
{
    public interface IEnemyInstance
    {
        Transform Transform { get; }
        EnemyConfig Stats { get; }
        IHealthComponent HealthComponent { get; }
    }
}