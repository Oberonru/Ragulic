using UniRx;
using UnityEngine;

namespace Core.CombatSystem
{
    public interface ICollisionHitBoxDetector
    {
        ISubject<IHitBox> OnHitBoxDetected { get; }
        ISubject<Collision> OnCollisionDetected { get; }
        ISubject<IHitBox> OnHitBoxExit { get; }
        ISubject<Collision> OnCollisionBoxExit { get; }
    }
}