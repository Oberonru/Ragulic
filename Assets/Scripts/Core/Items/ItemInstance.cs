using Core.Items.SO;
using UnityEngine;

namespace Core.Items
{
    public class ItemInstance : MonoBehaviour, IItemInstance
    {
        public ScriptableItem ScriptableItem => _scriptableItem;
        [SerializeField] private ScriptableItem _scriptableItem;
    }
}