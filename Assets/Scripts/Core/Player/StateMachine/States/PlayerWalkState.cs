using System.StateMachineSystem;
using UnityEngine;

namespace Core.Player.StateMachine.States
{
    public class PlayerWalkState : StateInstance<PlayerInstance>
    {
        public float Speed { get; set; }
        
        public override void Enter()
        {
            Debug.Log("PlayerWalkState Enter");
            Owner.Movement.Speed = Speed;
        }

        public override void Exit()
        {
            Debug.Log("PlayerWalkState Exit");
            Owner.Movement.Speed = Owner.Stats.WalkSpeed;
        }
    }
}