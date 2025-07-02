using Core.Items;
using UnityEngine;
using Zenject;

namespace Core.Factories.Items
{
    public class ItemFactory : IItemFactory
    {
        [Inject] private DiContainer _container;

        public IItemObject Create(ItemObject prefab, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            return _container.InstantiatePrefabForComponent<ItemObject>(prefab, position, rotation, parent);
        }
    }
}