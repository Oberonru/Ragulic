using UniRx;
using UnityEngine;

namespace Core.CombatSystem
{
    public class TriggerHitBoxDetector : MonoBehaviour, IHitBoxDetector
    {
        public ISubject<IHitBox> OnDetected => _onDetected;
        private Subject<IHitBox> _onDetected = new();

        public ISubject<IHitBox> OnHitBoxExit => _onHitBoxExit;
        private Subject<IHitBox> _onHitBoxExit = new();

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<IHitBox>(out IHitBox hitBox))
            {
                _onDetected?.OnNext(hitBox);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<IHitBox>(out IHitBox hitBox))
            {
                OnHitBoxExit?.OnNext(hitBox);
            }
        }
    }
}