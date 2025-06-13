using System.Collections.Generic;
using UnityEngine;

namespace System.StateMachineSystem
{
    public class StateMachineBase<T> : MonoBehaviour, IStateMachine<T> where T : UnityEngine.Component
    {
        private Dictionary<Type, StateInstance<T>> _states;
        private StateInstance<T> _currentState;
        private T _owner;

        private void Awake()
        {
            _owner = GetComponent<T>();
        }
        
        
    }
}