using System.Collections.Generic;
using UnityEngine;

namespace Worlds.Items.Inventory
{
    public class Inventory : MonoBehaviour
    {
        internal List<ItemData> items = new List<ItemData>();

        public delegate void ItemsModifiedDelegate(ItemData[] items);

        public ItemsModifiedDelegate itemsModifiedCallback;

        private void ItemsModified()
        {
            if (itemsModifiedCallback != null)
                itemsModifiedCallback.Invoke(items.ToArray());
        }

        /// <summary>
        /// Add an item to the inventory
        /// </summary>
        /// <param name="item"></param>
        public void AddItem(ItemData item)
        {
            items.Add(item);

            ItemsModified();
        }

        /// <summary>
        /// Remove an item from the inventory
        /// </summary>
        /// <param name="item">Item to remove</param>
        /// <param name="removeAll">Remove all the items of this type</param>
        public void RemoveItem(ItemData item, bool removeAll = false)
        {
            for (int i = items.Count - 1; i >= 0; i--)
            {
                if (items[i].key == item.key && items[i].name == item.name)
                {
                    // Remove this item
                    items.RemoveAt(i);

                    if (removeAll == false)
                        break;
                }
            }

            ItemsModified();
        }
    }
}