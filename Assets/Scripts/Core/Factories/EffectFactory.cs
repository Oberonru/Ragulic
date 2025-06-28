using Core.Effects;
using UnityEngine;
using Zenject;

namespace Core.Factories
{
    public class EffectFactory : IEffectFactory
    {
        [Inject] private DiContainer _container;
        
        public IEffectInstance Create(EffectInstance prefab, Vector3 position, Quaternion rotation, Transform parent = null)
        {
           return _container.InstantiatePrefabForComponent<EffectInstance>(prefab, position, rotation, parent);
        }
    }
}