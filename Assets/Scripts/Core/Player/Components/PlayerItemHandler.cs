using Core.Configs;
using Core.Items;
using Core.Items.Inventory;
using Core.Items.SO;
using UnityEngine;
using Zenject;

namespace Core.Player.Components
{
    public class PlayerItemHandler : MonoBehaviour
    {
        //инвентарь должен загружаться, нужен сейв лоад сервис
        //если не загрузился, создается новый
        [Inject] private InputConfig _inputConfig;
        
        private ItemsInventory _inventory;
        private IItemInstance _item;
        private ScriptableItem _scriptableItem;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IItemInstance item))
            {
                _scriptableItem = item.Item;
                _item = item;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out IItemInstance item))
            {
                _item = null;
                _scriptableItem = null;
            }
        }

        private void Update()
        {
            if (Input.GetKey(_inputConfig.Interaction) && _item != null)
            {
                var item = _scriptableItem.CreateItem();
                _inventory.AddItem(item);
                
                //проверка, если предмет добавлен - удаляем его и обнуляем переменные
            }
        }
    }
}