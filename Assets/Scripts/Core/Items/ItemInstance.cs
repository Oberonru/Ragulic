using Core.Items.SO;
using UnityEngine;

namespace Core.Items
{
    public class ItemInstance : MonoBehaviour, IItemInstance
    {
        public ScriptableItem Item => _scriptableItem;
        [SerializeField] private ScriptableItem _scriptableItem;
    }
}