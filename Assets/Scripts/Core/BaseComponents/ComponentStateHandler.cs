using UnityEngine;
using System.Linq;

namespace Core.BaseComponents
{
    public class ComponentStateHandler : MonoBehaviour
    {
        private IDisableComponent[] _disableComponents;

        private void Awake()
        {
            var selfComponents = GetComponents<IDisableComponent>();
            var parentComponents = GetComponentsInParent<IDisableComponent>(true);
            var childComponents = GetComponentsInChildren<IDisableComponent>(true);

            _disableComponents = selfComponents.
                Union(parentComponents).
                Union(childComponents).
                Distinct().
                ToArray();
        }

        public void DeactivateAllComponents()
        {
            foreach (var component in _disableComponents)
            {
                component.Disable();
            }
        }
    }
}