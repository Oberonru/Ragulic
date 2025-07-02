using Core.Items.SO;
using UnityEngine;

namespace Core.Items
{
    public class ItemObject : MonoBehaviour, IItemObject
    {
        public ScriptableItem Item => _scriptableItem;
        [SerializeField] private ScriptableItem _scriptableItem;
    }
}