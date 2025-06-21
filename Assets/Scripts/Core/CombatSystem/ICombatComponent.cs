namespace Core.CombatSystem
{
    public interface ICombatComponent
    {
        int Damage { get; }
        void SetDefaultDamage(int damage);
        void SetRandomDamage(int damage);
    }
}