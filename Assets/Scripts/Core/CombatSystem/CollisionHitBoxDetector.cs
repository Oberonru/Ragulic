using UniRx;
using UnityEngine;

namespace Core.CombatSystem
{
    public class CollisionHitBoxDetector : MonoBehaviour, ICollisionHitBoxDetector
    {
        public ISubject<IHitBox> OnDetected => _onDetected;
        private Subject<IHitBox> _onDetected = new();
        
        public ISubject<IHitBox> OnHitBoxExit => _onHitBoxExit;
        private Subject<IHitBox> _onHitBoxExit = new();

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.TryGetComponent<IHitBox>(out var hitBox))
            {
                _onDetected?.OnNext(hitBox);
            }
        }
    
        
        private void OnCollisionExit(Collision other)
        {
            if (other.gameObject.TryGetComponent<IHitBox>(out var hitBox))
            {
                _onHitBoxExit?.OnNext(hitBox);
            }
        }
    }
}