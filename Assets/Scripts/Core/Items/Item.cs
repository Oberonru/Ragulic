using Core.Items.SO;

namespace Core.Items
{
    public class Item
    {
        private ScriptableItem _scriptableItem;

        public Item(ScriptableItem item)
        {
            _scriptableItem = item;
        }
    }
}