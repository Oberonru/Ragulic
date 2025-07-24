using System;
using Core.Items;
using UniRx;
using UnityEngine;
using Zenject;

namespace Core.Factories.Items
{
    public class ItemFactory : IItemFactory
    {
        [Inject] private DiContainer _container;

        public IObservable<IItemInstance> OnSpawn => _onSpawn;
        private Subject<IItemInstance> _onSpawn = new();

        public IItemInstance Create(ItemInstance prefab, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            _onSpawn.OnNext(prefab);
            return _container.InstantiatePrefabForComponent<ItemInstance>(prefab, position, rotation, parent);
        }
    }
}