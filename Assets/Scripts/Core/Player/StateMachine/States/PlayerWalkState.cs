using System.StateMachineSystem;

namespace Core.Player.StateMachine.States
{
    public class PlayerWalkState : StateInstance<PlayerInstance>
    {
        public float Speed { get; set; }

        public override void Enter()
        {
            Owner.Controller.Speed = Speed;
        }

        public override void Exit()
        {
            Owner.Controller.Speed = 0;
        }
    }
}