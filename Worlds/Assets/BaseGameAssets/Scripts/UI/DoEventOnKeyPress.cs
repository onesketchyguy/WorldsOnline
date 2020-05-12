using UnityEngine;

namespace Worlds.UI
{
    public class DoEventOnKeyPress : MonoBehaviour
    {
        public KeyPressEvent[] keyPressEvents;

        [System.Serializable]
        public class KeyPressEvent
        {
            public string ButtonName = "Cancel";
            public KeyCode keyCode = KeyCode.None;

            public UnityEngine.Events.UnityEvent @event;
        }

        private void Update()
        {
            foreach (var item in keyPressEvents)
            {
                if (string.IsNullOrWhiteSpace(item.ButtonName))
                {
                    if (Input.GetKeyUp(item.keyCode))
                        item.@event.Invoke();
                }
                else
                if (Input.GetButtonUp(item.ButtonName))
                    item.@event.Invoke();
            }
        }
    }
}