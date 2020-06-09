using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Worlds.Items.Inventory
{
    public class InventoryDisplay : MonoBehaviour
    {
        [Tooltip("Inventory to display")]
        public Inventory inventory;

        public GameObject buttonPrefab;

        private GameObject[] buttons;

        private void Start()
        {
            inventory.itemsModifiedCallback += UpdateDisplay;
            UpdateDisplay(inventory.items.ToArray()); // TO DO: Remove when the items get loaded
        }

        private void UpdateDisplay(ItemData[] items)
        {
            foreach (var item in items)
            {
                CreateButtonFromItem(item);
            }
        }

        private void CreateButtonFromItem(ItemData item)
        {
            var obj = Instantiate(buttonPrefab, transform);
            var text = obj.GetComponent<UnityEngine.UI.Text>();
            text.text = item.name;

            // Set the image

            // Set the on click event
        }
    }
}