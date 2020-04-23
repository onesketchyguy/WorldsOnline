using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Worlds.UI
{
    public class Enabler : MonoBehaviour
    {
        public GameObject[] objectsToEnable;

        public bool enableOnLevelWasLoaded = true;

        private void Start()
        {
            if (enableOnLevelWasLoaded)
            {
                SetItems(true);
            }
        }

        public void Toggle()
        {
            SetItems(!objectsToEnable.FirstOrDefault().activeSelf);
        }

        public void SetItems(bool value)
        {
            foreach (var item in objectsToEnable)
            {
                item.SetActive(value);
            }
        }
    }
}