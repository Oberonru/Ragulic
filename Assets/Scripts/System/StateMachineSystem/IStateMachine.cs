namespace System.StateMachineSystem
{
    public interface IStateMachine<T> where T : UnityEngine.Component
    {
        void SetState(Type type);
        void SetState<TState>() where TState : StateInstance<T>;
        void SetState(StateInstance<T> instance);

        object GetState(Type type);
        TState GetState<TState>() where TState: StateInstance<T>;
    }
}