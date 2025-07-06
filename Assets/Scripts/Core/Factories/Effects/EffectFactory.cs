using System.Collections.Generic;
using System.Pool;
using Core.Effects;
using UnityEngine;
using Zenject;

namespace Core.Factories.Effects
{
    public class EffectFactory : IEffectFactory
    {
        [Inject] private DiContainer _container;
        private Dictionary<EffectInstance, PoolMono<EffectInstance>> _pool = new();

        private const int Count = 1;

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

                return freeElement;
            }
            else
            {
                var freeElement =  pool.GetFreeElement();
                freeElement.transform.position = position;
                freeElement.transform.rotation = rotation;
                
                return freeElement;
            }
        }
    }
}