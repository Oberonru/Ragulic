using System.Collections.Generic;
using System.Linq;
using System.StateMachineSystem.SO;
using UnityEngine;

namespace System.StateMachineSystem
{
    public class StateMachineBase<T> : MonoBehaviour, IStateMachine<T> where T : UnityEngine.Component
    {
        [SerializeField] private StateContainer<T> _stateContainer;

        private Dictionary<Type, StateInstance<T>> _states;
        private StateInstance<T> _activeState;
        private T _owner;
        

        private void Awake()
        {
            MapInitialize();
        }

        private void MapInitialize()
        {
            _owner = GetComponent<T>();

            if (_stateContainer is null)
                throw new NullReferenceException($"state container {_stateContainer.name} is not found");

            IEnumerable<StateInstance<T>> stateInstances = _stateContainer.GetStateInstances();

            foreach (var instance in stateInstances)
            {
                instance.SetOwner(_owner);
                var type = instance.GetType();
                _states.Add(type, instance);
            }
        }

        public void SetDefaultState()
        {
            var firstValue = _states.First().Value;
            var type = firstValue.GetType();

            SetState(type);
        }

        public void SetState(Type type)
        {
            if (_states.TryGetValue(type, out var instance))
            {
                _activeState?.Exit();
                _activeState = instance;
                _activeState.Enter();
            }
            else
            {
                throw new NullReferenceException($"State {type.Name} not exist in State machine");
            }
        }

        public void SetState<TState>() where TState : StateInstance<T>
        {
            Type type = typeof(TState);
            SetState(type);
        }

        public void SetState(StateInstance<T> instance)
        {
            Type type = instance.GetType();
            SetState(type);
        }

        public object GetState(Type type)
        {
            if (_states.TryGetValue(type, out var value))
            {
                return value;
            }
            else
            {
                throw new NullReferenceException($"State with type {type.Name} not found");
            }
        }

        public TState GetState<TState>() where TState : StateInstance<T>
        {
            Type type = typeof(TState);
            return GetState(type) as TState;
        }
    }
}