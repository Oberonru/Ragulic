using System.StateMachineSystem;
using Core.Player.StateMachine.States;

namespace Core.Player.StateMachine
{
    public class PlayerStateMachine : StateMachineBase<PlayerInstance>, IPlayerState
    {
        public void SetWalking(float speed)
        {
            var state = GetState<PlayerWalkState>();
            state.Speed = speed;
            SetState(state);
        }

        public void SetRunning(float speed)
        {
            var state = GetState<PlayerRunState>();
            state.Speed = speed;
            SetState(state);
        }

        public void SetCrouch(float speed)
        {
            var state = GetState<PlayerPanicState>();
            state.Speed = speed;
            SetState(state);
        }

        public void SetPanic(float speed)
        {
            var state = GetState<PlayerPanicState>();
            state.Speed = speed;
            SetState(state);
        }
    }
}