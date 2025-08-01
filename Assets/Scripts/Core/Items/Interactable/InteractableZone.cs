using System.UI;
using Core.Configs;
using Core.Player;
using UniRx;
using UnityEngine;
using Zenject;

namespace Core.Items.Interactable
{
    public class InteractableZone : MonoBehaviour
    {
        [Inject] private KeyConfig _config;
        
        [SerializeField] private float interactTime = 1f;
        [SerializeField] private CircleSlider _circleSlider;
        
        public ISubject<bool> CanInteract => _canInteract;
        private Subject<bool> _canInteract = new();
        
        public ISubject<float> OnInteracting => _onInteracting;
        private Subject<float> _onInteracting = new();
        
        public ISubject<bool> EndInteracting => _endInteracting;
        private Subject<bool> _endInteracting = new();
        
        private bool _isInteracting;
        private float _btnDownTime;

        private void Update()
        {
            if (Input.GetKeyDown(_config.Interaction))
            {
                ApplyInteractionProgress();
            }
            else DisableInteractionProgress();
        }

        private void ApplyInteractionProgress()
        {
            _btnDownTime += Time.deltaTime;
            
            _circleSlider.SetValue(_btnDownTime, interactTime);

            if (_btnDownTime >= interactTime)
            {
                _endInteracting.OnNext(true);
                _btnDownTime = 0;
            }
        }
        
        private void DisableInteractionProgress() {}

        public void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<IPlayerInstance>(out var player))
            {
                _canInteract.OnNext(true);
                _isInteracting = true;
            }
        }

        public void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<IPlayerInstance>(out var player))
            {
                _canInteract.OnNext(false);
                _isInteracting = false;
            }
        }
    }
}