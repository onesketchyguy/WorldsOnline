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

        private bool contains(GameObject obj)
        {
            for (int i = 0; i < tabs.Length; i++)
                if (tabs[i] == obj) return true;

            return false;
        }

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
            if (contains(tab) == false)
            {
                Debug.LogError($"{gameObject.name}.{nameof(Tabber)} does not contain tab {tab.name}!");
                return;
            }

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