using System.SO;

namespace System.StateMachineSystem.SO
{
    public abstract class ScriptableState<T> : ScriptableObjectIdentity where T : UnityEngine.Component
    {
        public abstract StateInstance<T> GetInstance();
    }
}