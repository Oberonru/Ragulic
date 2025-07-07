using Core.Items.SO;
using Core.Player;
using UnityEngine;
using Zenject;

namespace Core.Items
{
    public class ItemInstance : MonoBehaviour, IItemInstance
    {
        [Inject] private IPlayerInstance _player;
        
        public ScriptableItem ScriptableItem => _scriptableItem;
        [SerializeField] private ScriptableItem _scriptableItem;

        public void Interact()
        {
            var item = _scriptableItem.CreateItem();
            _player.InventoryHandler.Inventory.AddItem(item);
            
            Destroy(gameObject);
        }
    }
}