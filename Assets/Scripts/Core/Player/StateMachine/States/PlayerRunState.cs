using System.StateMachineSystem;
using UnityEngine;

namespace Core.Player.StateMachine.States
{
    public class PlayerRunState : StateInstance<PlayerInstance>
    {
        public float Speed { get; set; }
        private Vector3 _velocity;
        
        public override void Enter()
        {
            Owner.PlayerController.Speed = Speed;
        }

        public override void Exit()
        {
            Owner.PlayerController.Speed = Owner.Stats.WalkSpeed;
        }
    }
}