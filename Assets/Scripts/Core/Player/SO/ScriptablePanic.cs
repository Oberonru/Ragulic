using System.StateMachineSystem;
using System.StateMachineSystem.SO;
using Core.Player.StateMachine.States;
using UnityEngine;

namespace Core.Player.SO
{
    [CreateAssetMenu(menuName = "StateMachine/Player/ScriptablePanic", fileName = "PanicState")]
    public class ScriptablePanic : ScriptableState<PlayerInstance>
    {
        public override StateInstance<PlayerInstance> GetInstance()
        {
            return new PlayerPanicState();
        }
    }
}