using System.Factories;
using Core.Items;

namespace Core.Factories.Items
{
    public interface IItemFactory : IMonoBehaviorFactory<ItemObject, IItemObject>
    {
    }
}