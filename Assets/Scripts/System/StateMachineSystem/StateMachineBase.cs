using UnityEngine;

namespace System.StateMachineSystem
{
    public class StateMachineBase<T> : MonoBehaviour, IStateMachine<T> where T : UnityEngine.Component
    {
    }
}