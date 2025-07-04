using System.StateMachineSystem;
using UnityEngine;

namespace Core.Player.StateMachine.States
{
    public class PlayerRunState : StateInstance<PlayerInstance>
    {
        public float Speed { get; set; }

        public override void Enter()
        {
            Debug.Log("PlayerRunState Enter");
            Owner.Movement.Speed = Speed;
            Owner.Movement.IsCrouch = false;
        }

        public override void Exit()
        {
            Owner.Movement.Speed = Owner.Stats.WalkSpeed;
            Debug.Log("PlayerRunState Exit Owner.Stats.WalkSpeed" +  Owner.Movement.Speed) ;
        }
    }
}