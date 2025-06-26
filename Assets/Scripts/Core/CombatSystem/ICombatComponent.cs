namespace Core.CombatSystem
{
    public interface ICombatComponent
    {
        int Damage { get; }
        void DefaultDamage(int damage);
        void RandomDamage(int damage);
    }
}