using Core.BaseComponents;
using UniRx;
using UnityEngine;

namespace Core.Handlers
{
    public class DisableComponentAfterDeathHandler : MonoBehaviour
    {
        [SerializeField] private HealthComponent _health;
        [SerializeField] private ComponentStateHandler _stateHandler;

        private void OnEnable()
        {
            _health.OnDead.Subscribe(_ => _stateHandler.DeactivateAllComponents()).
                AddTo(this);
        }
    }
}