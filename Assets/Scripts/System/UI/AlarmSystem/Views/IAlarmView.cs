using System.Factories;
using UnityEngine;

namespace System.UI.AlarmSystem.Views
{
    public interface IAlarmView : IFactoryObject
    {
        void ShowInfo(Sprite icon, string alarmInfo);
    }
}