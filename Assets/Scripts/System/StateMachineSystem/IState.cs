using UnityEngine;

namespace System.StateMachineSystem
{
    public interface IState<T> where T : Component
    {
        T Owner { get;  }
        void Enter();
        void Exit();
    }
}