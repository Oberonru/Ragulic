using Core.Items.SO;

namespace Core.Items
{
    public class Item
    {
        public ScriptableItem ScriptableItem => _scriptableItem;
        private ScriptableItem _scriptableItem;

        public Item(ScriptableItem item)
        {
            _scriptableItem = item;
        }
    }
}