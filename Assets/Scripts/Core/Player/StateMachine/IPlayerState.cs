using System.StateMachineSystem;

namespace Core.Player.StateMachine
{
    public interface IPlayerState
    {
        void SetWalkSpeed(float speed);
        void SetRunnSpeed(float speed);
        void SetCrouchSpeed(float speed);
        void SetPanicSpeed(float speed);
    }
}