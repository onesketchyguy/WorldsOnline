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
            if (string.IsNullOrWhiteSpace(ButtonName))
            {
                if (Input.GetKeyUp(keyCode))
                    @event.Invoke();
            }
            else
            if (Input.GetButtonUp(ButtonName))
                @event.Invoke();
        }
    }
}