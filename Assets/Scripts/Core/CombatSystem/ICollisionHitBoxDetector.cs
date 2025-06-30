using UniRx;

namespace Core.CombatSystem
{
    public interface ICollisionHitBoxDetector
    {
        ISubject<IHitBox> OnHitBoxExit { get; }

        ISubject<IHitBox> OnDetected { get; }
    }
}