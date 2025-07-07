using Core.Configs;
using Core.Items;
using UnityEngine;
using Zenject;

namespace Core.Player.Components
{
    public class PlayerItemHandler : MonoBehaviour
    {
        [Inject] private InputConfig _inputConfig;
        [SerializeField] private PlayerInstance _player;

        private IInteractableObject _interactableObject;

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
            if (_interactableObject != null && Input.GetKey(_inputConfig.Interaction))
            {
                _interactableObject.Interact();
                _interactableObject = null;
            }
        }

        private void OnValidate()
        {
            if (_player is null) _player = GetComponent<PlayerInstance>();
        }
    }
}