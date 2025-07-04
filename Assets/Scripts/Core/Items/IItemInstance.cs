using System.Factories;
using Core.Items.SO;

namespace Core.Items
{
    public interface IItemInstance : IFactoryObject
    {
        ScriptableItem ScriptableItem { get; }
    }
}