using System.StateMachineSystem;
using System.StateMachineSystem.SO;
using Core.Player.StateMachine.States;
using UnityEngine;

namespace Core.Player.SO
{
    [CreateAssetMenu(menuName = "StateMachine/Player/ScriptableWalking", fileName = "WalkingState")]
    public class ScriptableWalking : ScriptableState<PlayerInstance>
    {
        public override StateInstance<PlayerInstance> GetInstance()
        {
            return new PlayerWalkState();
        }
    }
}