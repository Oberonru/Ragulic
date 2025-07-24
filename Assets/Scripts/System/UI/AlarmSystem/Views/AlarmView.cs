using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace System.UI.AlarmSystem.Views
{
    public class AlarmView : MonoBehaviour,  IAlarmView
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _alarmInfo;
        
        public void ShowInfo(Sprite icon, string alarmInfo)
        {
            _icon.sprite = icon;
            _alarmInfo.text = alarmInfo;
        }
    }
}