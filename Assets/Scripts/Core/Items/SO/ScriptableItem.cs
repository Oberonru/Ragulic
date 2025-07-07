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
        
        [BoxGroup("Item params")]
        [PropertyTooltip("Цена предмета")]
        [LabelText("Item price")]
        [SerializeField] 
        private int _price;
        
        public ItemType ItemType => _itemType;

        public int Price => _price;
        
        public Item CreateItem()
        {
            return new Item(this);
        }
    }
}