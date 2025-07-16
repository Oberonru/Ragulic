using System.StateMachineSystem;

namespace Core.Player.StateMachine.States
{
    public class PlayerCrouchState : StateInstance<PlayerInstance>
    {
        public float Speed { get; set; }

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