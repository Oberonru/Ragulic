using UniRx;

namespace Core.BaseComponents
{
    public interface IHealthComponent
    {
        bool IsAllive { get; }
        int MaxHealth { get; set; }
        int CurrentHealth { get; set; }
        ISubject<object> OnHit { get; }
        ISubject<int> OnHealthChanged { get; }
        ISubject<Unit> OnDead { get; }

        void TakeDamage(int amount, object damager = null);
        void Regeneration(int amount);
    }
}