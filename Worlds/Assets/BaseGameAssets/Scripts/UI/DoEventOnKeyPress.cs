using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Worlds.UI
{
    public class DoEventOnKeyPress : MonoBehaviour
    {
        public string ButtonName = "Cancel";
        public KeyCode keyCode = KeyCode.None;

        public UnityEngine.Events.UnityEvent @event;

        private void Update()
        {
            if (Input.GetButtonUp(ButtonName) || Input.GetKeyUp(keyCode))
            {
                @event.Invoke();
            }
        }
    }
}