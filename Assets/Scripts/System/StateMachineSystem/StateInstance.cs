using UnityEngine;

namespace System.StateMachineSystem
{
    public abstract class StateInstance<T> : IState<T> where T : Component
    {
        public T Owner { get; private set; }
        public abstract void Enter();
        public abstract void Exit();

        public void SetOwner(T owner)
        {
            Owner = owner;
        } 
    }
}