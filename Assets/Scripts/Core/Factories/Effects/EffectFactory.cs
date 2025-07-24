using System;
using System.Collections.Generic;
using System.Pool;
using Core.Effects;
using UniRx;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace Core.Factories.Effects
{
    public class EffectFactory : IEffectFactory
    {
        [Inject] private DiContainer _container;
        private Dictionary<EffectInstance, PoolMono<EffectInstance>> _pool = new();

        private const int Count = 1;
        public IObservable<IEffectInstance> OnSpawn => _onSpawn;
        private Subject<IEffectInstance> _onSpawn = new();

        public IEffectInstance Create(EffectInstance prefab, Vector3 position, Quaternion rotation,
            Transform parent = null)
        {
            if (!_pool.TryGetValue(prefab, out PoolMono<EffectInstance> pool))
            {
                var instance = Object.Instantiate(prefab, position, rotation, parent);

                var newPool = new PoolMono<EffectInstance>
                    (prefab, instance.transform, _container, Count, true, true);

                _pool.Add(prefab, newPool);

                var freeElement = newPool.GetFreeElement();
                freeElement.transform.position = position;
                freeElement.transform.rotation = rotation;

                _onSpawn.OnNext(freeElement);

                return freeElement;
            }
            else
            {
                var freeElement = pool.GetFreeElement();
                freeElement.transform.position = position;
                freeElement.transform.rotation = rotation;

                _onSpawn.OnNext(freeElement);

                return freeElement;
            }
        }
    }
}