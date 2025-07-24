using System.UI.AlarmSystem.Views;
using UnityEngine;

namespace DevTools
{
    public class TestAlarmView : MonoBehaviour
    {
        [SerializeField] private AlarmGroupView _alarmGroupView;
        [SerializeField] private Sprite _icon;
        [SerializeField] private string _alarmInfo;
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                _alarmGroupView.ShowAlarmInfo(_icon, _alarmInfo);
            }
        }
    }
}