using Core.Player;
using UniRx;
using UnityEngine;

namespace Core.Items.Components
{
    public class InteractableZone : MonoBehaviour, IInteractableZone
    {
        [SerializeField] private CapsuleCollider _collider;

        public ISubject<Unit> OnZoneEntered => _onZoneEntered;
        private Subject<Unit> _onZoneEntered = new();

        public ISubject<Unit> OnZoneExited => _onZoneExited;
        private Subject<Unit> _onZoneExited = new();

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IPlayerInstance player))
            {
                _onZoneEntered.OnNext(Unit.Default);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out IPlayerInstance player))
            {
                _onZoneExited.OnNext(Unit.Default);
            }
        }

        private void OnValidate()
        {
            if (_collider is null) _collider = GetComponent<CapsuleCollider>();
        }

        private void OnDrawGizmos()
        {
            if (_collider is null) return;
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, _collider.radius);
        }
    }
}