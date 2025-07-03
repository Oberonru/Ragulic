using System.Collections.Generic;

namespace Core.Items.Inventory
{
    public class ItemsInventory
    {
        private List<Item> _items = new();

        public void AddItem(Item item)
        {
            if (item is null) return;
            
            _items.Add(item);
        }

        public void RemoveItem(Item item)
        {
            if (_items.Contains(item))
            {
                _items.Remove(item);
            }
        }

        public void GiveItem(Item item)
        {
        }
    }
}