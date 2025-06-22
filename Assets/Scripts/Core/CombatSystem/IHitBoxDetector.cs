using UniRx;

namespace Core.CombatSystem
{
    public interface IHitBoxDetector
    {
        ISubject<IHitBox> OnDetected { get; }
        ISubject<IHitBox> OnHitBoxExit { get; }
    }
}