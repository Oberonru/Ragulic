using System.StateMachineSystem;
using System.StateMachineSystem.SO;
using Core.Player.StateMachine.States;
using UnityEngine;

namespace Core.Player.SO
{
    [CreateAssetMenu(menuName = "StateMachine/Player/ScriptableCrouch", fileName = "CrouchState")]
    public class ScriptableCrouch : ScriptableState<PlayerInstance>
    {
        public override StateInstance<PlayerInstance> GetInstance()
        {
            return new PlayerCrouchState();
        }
    }
}