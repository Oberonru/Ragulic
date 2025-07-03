using Core.SO;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Core.Items.SO
{
    [CreateAssetMenu(menuName = "Items/SO/ScriptableItem", fileName = "ScriptableItem")]
    public class ScriptableItem : ScriptableBaseEntityData
    {
        [BoxGroup("Item params")]
        [PropertyTooltip("Тип предмета")]
        [LabelText("Item type")]
        [SerializeField] 
        private ItemType _itemType;
        
        public ItemType ItemType => _itemType;

        public Item CreateItem()
        {
            return new Item(this);
        }
    }
}