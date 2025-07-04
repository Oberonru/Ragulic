using System.StateMachineSystem;
using UnityEngine;

namespace Core.Player.StateMachine.States
{
    public class PlayerCrouchState : StateInstance<PlayerInstance>
    {
        public float Speed { get; set; }

        public override void Enter()
        {
            Debug.Log("PlayerCrouchState Enter");
            Owner.Movement.Speed = Speed;
        }

        public override void Exit()
        {
            Debug.Log("PlayerCrouchState Exit");
            Owner.Movement.Speed = Owner.Stats.WalkSpeed;
        }
    }
}