using Core.Configs;
using Core.Items;
using Core.Items.Inventory;
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
        private IItemInstance _instance;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IItemInstance item))
            {
                _instance = item;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out IItemInstance item))
            {
                _instance = null;
            }
        }

        private void Update()
        {
            if (Input.GetKey(_inputConfig.Interaction) && _instance != null)
            {
                //Как быть с зонами, здесь взаимодействие с ItemInstance, но надо с интерактабл зоной?
                var item = _instance.ScriptableItem.CreateItem();
                _inventory.AddItem(item);

                //проверка, если предмет добавлен - удаляем его и обнуляем переменные
            }
        }
    }
}