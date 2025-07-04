using System.StateMachineSystem;
using System.StateMachineSystem.SO;
using Core.Player.StateMachine.States;
using UnityEngine;

namespace Core.Player.SO
{
    [CreateAssetMenu(menuName = "StateMachine/Player/ScriptableRunning", fileName = "RunningState")]
    public class ScriptableRunning : ScriptableState<PlayerInstance>
    {
        public override StateInstance<PlayerInstance> GetInstance()
        {
            return new PlayerRunState();
        }
    }
}