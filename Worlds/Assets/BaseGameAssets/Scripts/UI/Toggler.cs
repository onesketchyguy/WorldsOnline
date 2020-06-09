using UnityEngine;

namespace Worlds.UI
{
    public class Toggler : MonoBehaviour
    {
        public bool defaultValue = true;

        public GameObject objectToToggle;

        private void Start()
        {
            if (objectToToggle.activeSelf != defaultValue) Toggle();
        }

        public void Toggle()
        {
            objectToToggle.SetActive(!objectToToggle.activeSelf);
        }
    }
}