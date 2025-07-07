using Core.Items.Inventory;
using UnityEngine;

namespace Core.Player.Components
{
    public class InventoryPlayerHandler : MonoBehaviour
    {
        public ItemsInventory Inventory => _inventory;
        private ItemsInventory _inventory;

        private void Awake()
        {
            //инвентарь загружается из БД или еще откуда то, если нет - создается по новой
            _inventory = new ItemsInventory();
        }
    }
}