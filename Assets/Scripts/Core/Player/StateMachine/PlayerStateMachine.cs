using System.StateMachineSystem;
using Core.Player.StateMachine.States;

namespace Core.Player.StateMachine
{
    public class PlayerStateMachine : StateMachineBase<PlayerInstance>, IPlayerState
    {
        public void SetWalkSpeed(float speed)
        {
            var state = GetState<PlayerWalkState>();
            state.Speed = speed;
            SetState(state);
        }   

        public void SetRunnSpeed(float speed)
        {
            var state = GetState<PlayerRunState>();
            state.Speed = speed;
            SetState(state);
        }

        public void SetCrouchSpeed(float speed)
        {
            var state = GetState<PlayerCrouchState>();
            state.Speed = speed;
            SetState(state);
        }

        public void SetPanicSpeed(float speed)
        {
            var state = GetState<PlayerPanicState>();
            state.Speed = speed;
            SetState(state);
        }
    }
}