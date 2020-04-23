using System.Linq;
using UnityEngine;

namespace Worlds.UI
{
    /// <summary>
    /// Create a toggler for an array of objects.
    /// </summary>
    public class Tabber : MonoBehaviour
    {
        [Tooltip("Tabs to switch through. Will automatically set the first tab to the default tab.")]
        public GameObject[] tabs;

        private void Start() => ToggleToTab(tabs.FirstOrDefault()); // Set the current tab to the default tab

        /// <summary>
        /// Will toggle back to the default front tab
        /// </summary>
        /// <param name="tab"></param>
        public void ToggleFromTab() => ToggleToTab(tabs.FirstOrDefault());

        /// <summary>
        /// Will toggle to a specified tab
        /// </summary>
        /// <param name="tab"></param>
        public void ToggleToTab(GameObject tab)
        {
            if (tab.activeSelf == true && tab != tabs.FirstOrDefault())
            {
                ToggleFromTab(); // Return to the first tab if the user is requesting to go back to the same tab they are on.
                return;
            }

            foreach (var item in tabs)
                item.SetActive(item == tab);
        }
    }
}