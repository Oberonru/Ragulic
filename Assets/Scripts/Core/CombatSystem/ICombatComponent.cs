namespace Core.CombatSystem
{
    public interface ICombatComponent
    {
        int Damage { get; }
        void SetDamage(int damage);
    }
}