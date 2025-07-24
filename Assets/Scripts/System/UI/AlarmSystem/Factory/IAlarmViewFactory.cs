using System.Factories;
using System.UI.AlarmSystem.Views;

namespace System.UI.AlertSystem.Factory
{
    public interface IAlarmViewFactory : IMonoBehaviorFactory<AlarmView, IAlarmView>
    {
    }
}