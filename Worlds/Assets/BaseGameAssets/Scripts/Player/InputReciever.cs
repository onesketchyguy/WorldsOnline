using Mirror;
using UnityEngine;

namespace Worlds.Player
{
    public class InputReceiver : NetworkBehaviour
    {
        internal IInput inputManager;

        internal void SetInputManager()
        {
            inputManager = GetComponent<IInput>();

            if (inputManager == null)
            {
                // Try find one.

                var objects = FindObjectsOfType<NetworkBehaviour>();

                foreach (var item in objects)
                {
                    if (item.isLocalPlayer)
                    {
                        var obj = (IInput)item;

                        if (obj != null)
                        {
                            inputManager = obj;
                        }
                    }
                }

                if (inputManager == null)
                {
                    Debug.LogError($"Unable to find {nameof(IInput)} within the scene!");
                    Destroy(this);
                }
            }
        }

        internal virtual void Start()
        {
            SetInputManager();
        }
    }
}