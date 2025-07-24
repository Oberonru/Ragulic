using System.Pool;
using System.UI.AlarmSystem.Views;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace System.UI.AlertSystem.Factory
{
    public class AlarmViewFactory : IAlarmViewFactory
    {
        [Inject] private DiContainer _di;
        private PoolMono<AlarmView> _pool;

        public IObservable<IAlarmView> OnSpawn => _onSpawn;
        private Subject<AlarmView> _onSpawn = new();

        public IAlarmView Create(AlarmView prefab, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            if (_pool == null)
            {
                SceneManager.sceneUnloaded += OnSceneUnloaded;
                _pool = new PoolMono<AlarmView>(prefab, null, _di, 10, true);
            }

            var view = _pool.GetFreeElement();

            if (parent != null)
            {
                view.transform.SetParent(parent, false);
            }

            _onSpawn.OnNext(view);

            return view;
        }

        private void OnSceneUnloaded(Scene scene)
        {
            _pool = null;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }
    }
}