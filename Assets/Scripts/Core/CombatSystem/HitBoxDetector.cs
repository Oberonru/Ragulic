using UniRx;
using UnityEngine;

namespace Core.CombatSystem
{
    public class HitBoxDetector : MonoBehaviour
    {
        public ISubject<IHitBox> OnDetected => _onDetected;
        private Subject<IHitBox> _onDetected = new();

        public ISubject<IHitBox> OnHitBoxExit => _onHitBoxExit;
        private Subject<IHitBox> _onHitBoxExit = new();

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<IHitBox>(out var hitBox))
            {
                OnDetected?.OnNext(hitBox);
                Debug.Log("Trigger enter");
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<IHitBox>(out var hitBox))
            {
                OnHitBoxExit?.OnNext(hitBox);
                Debug.Log("Trigger exit");

            }
        }
    }
}