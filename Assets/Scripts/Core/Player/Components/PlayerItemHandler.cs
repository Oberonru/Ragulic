using Core.Configs;
using Core.Items;
using UniRx;
using UnityEngine;
using Zenject;

namespace Core.Player.Components
{
    public class PlayerItemHandler : MonoBehaviour
    {
        [Inject] private KeyConfig _keyConfig;

        private IInteractableObject _interactableObject;

        private Subject<Unit> _onInteract = new();
        public ISubject<Unit> OnInteract => _onInteract;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IInteractableObject interactableObject))
            {
                _interactableObject = interactableObject;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out IInteractableObject interactableObject))
            {
                _interactableObject = null;
            }
        }

        private void Update()
        {
            if (_interactableObject != null && Input.GetKey(_keyConfig.Interaction))
            {
                _onInteract.OnNext(Unit.Default);
                _interactableObject.Interact();
            }
        }
    }
}