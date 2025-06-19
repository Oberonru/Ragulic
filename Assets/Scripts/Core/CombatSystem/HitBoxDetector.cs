using UniRx;
using UnityEngine;

namespace Core.CombatSystem
{
    public class HitBoxDetector : MonoBehaviour
    {
        public Subject<IHitBox> OnDetected => _onDetected;
        private Subject<IHitBox> _onDetected = new();

        public Subject<IHitBox> OnHitBoxExit => _onHitBoxExit;
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