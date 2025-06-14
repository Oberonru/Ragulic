using System.Collections.Generic;
using System.Linq;
using System.SO;
using UnityEngine;

namespace System.StateMachineSystem.SO
{
    public class StateContainer<T> : ScriptableObjectIdentity where T : UnityEngine.Component
    {
        [SerializeField] private StateContainer<T> _parentContainer;
        [SerializeField] private ScriptableState<T>[] _states;

        public IEnumerable<StateInstance<T>> GetStateInstances()
        {
            IEnumerable<ScriptableState<T>> allStates = GetParentStates().Concat(_states);
            List<StateInstance<T>> instances = new();

            foreach (var state in allStates)
            {
                var instance = state.GetInstance();
                instances.Add(instance);
            }

            return instances;
        }

        private IEnumerable<ScriptableState<T>> GetParentStates()
        {
            return _parentContainer != null ? _states : Enumerable.Empty<ScriptableState<T>>();
        }
    }
}