namespace Core.Player.StateMachine
{
    public interface IPlayerState
    {
        void SetWalking(float speed);
        void SetRunning(float speed);
        void SetCrouch(float speed);
        void SetPanic(float speed);
    }
}