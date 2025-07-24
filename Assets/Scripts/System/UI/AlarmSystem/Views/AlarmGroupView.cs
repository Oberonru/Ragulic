using System.UI.AlertSystem.Factory;
using Core.Configs.UI.Views;
using UnityEngine;
using Zenject;

namespace System.UI.AlarmSystem.Views
{
    public class AlarmGroupView : MonoBehaviour
    {
        [Inject] private AlarmViewConfig _config;
        [Inject] private IAlarmViewFactory _factory;
        [SerializeField] private AlarmView _view;
        [SerializeField] private RectTransform _container;

        public void ShowAlarmInfo(Sprite icon, string alarmInfo)
        {
            var view = _factory.Create(_config.Prefab, Vector3.zero, Quaternion.identity, _container);
            view.ShowInfo(icon, alarmInfo);
        }
    }
}