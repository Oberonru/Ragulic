using UniRx;

namespace Core.Items.Components
{
    public interface IInteractableZone
    {
        ISubject<Unit> OnZoneEntered { get; }
        ISubject<Unit> OnZoneExited { get; }
    }
}