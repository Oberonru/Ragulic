using UniRx;
using UnityEngine;

namespace Core.CombatSystem
{
    public class CollisionHitBoxDetector : MonoBehaviour, ICollisionHitBoxDetector
    {
        public ISubject<IHitBox> OnHitBoxDetected => _onHitBoxDetected;
        private Subject<IHitBox> _onHitBoxDetected = new();

        public ISubject<Collision> OnCollisionDetected => _onCollisionDetected;
        private Subject<Collision> _onCollisionDetected = new();
        public ISubject<IHitBox> OnHitBoxExit => _onHitBoxExit;
        private Subject<IHitBox> _onHitBoxExit = new();

        public ISubject<Collision> OnCollisionBoxExit => _onCollisionBoxExit;
        private Subject<Collision> _onCollisionBoxExit = new();

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.TryGetComponent<IHitBox>(out var hitBox))
            {
                _onHitBoxDetected?.OnNext(hitBox);
                _onCollisionDetected?.OnNext(other);
            }
        }

        private void OnCollisionExit(Collision other)
        {
            if (other.gameObject.TryGetComponent<IHitBox>(out var hitBox))
            {
                _onHitBoxExit?.OnNext(hitBox);
                _onCollisionBoxExit?.OnNext(other);
            }
        }
    }
}