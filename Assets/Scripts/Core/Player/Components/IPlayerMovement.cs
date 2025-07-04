namespace Core.Player.Components
{
    public interface IPlayerMovement
    {
        float Speed { get; set; }
        bool IsRunning { get; set; }
        public bool IsCrouch { get; set; }
        bool IsPanic { get; set; }
    }
}