using System.StateMachineSystem;

namespace Core.Player.StateMachine.States
{
    public class PlayerPanicState : StateInstance<PlayerInstance>
    {
        public float Speed { get; set; }
        
        public override void Enter()
        {
            Owner.Movement.Speed = Speed;
        }

        public override void Exit()
        {
            Owner.Movement.Speed = Owner.Stats.WalkSpeed;
        }
    }
}