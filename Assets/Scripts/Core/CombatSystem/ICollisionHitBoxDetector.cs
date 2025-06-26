using Core.Enemies.Dto;
using UniRx;

namespace Core.CombatSystem
{
    public interface ICollisionHitBoxDetector
    {
        ISubject<IHitBox> OnHitBoxExit { get; }

        ISubject<IAttackDto> OnDetected { get; }
    }
}