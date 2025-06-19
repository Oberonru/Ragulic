using Core.BaseComponents;

namespace Core.CombatSystem
{
    public interface IHitBox
    {
        IHealthComponent HealthComponent { get; }
    }
}