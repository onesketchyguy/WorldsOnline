using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Worlds
{
    using Player;

    public class OnLocalPlayerTriggerEvent : MonoBehaviour
    {
        private PlayerController player;

        public KeyCode key;

        public UnityEngine.Events.UnityEvent OnEnterEvent;
        public UnityEngine.Events.UnityEvent OnExitEvent;
        public UnityEngine.Events.UnityEvent OnKeyPressEvent;

        private void Update()
        {
            if (player != null)
            {
                // Check for an input
                if (key == KeyCode.None || Input.GetKeyDown(key))
                {
                    // Attach the events
                    OnKeyPressEvent.Invoke();
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            var _player = other.GetComponent<PlayerController>();
            if (_player != null && _player.isLocalPlayer)
            {
                player = _player;
                OnEnterEvent.Invoke();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            var _player = other.GetComponent<PlayerController>();
            if (_player == player)
            {
                player = null;
                OnExitEvent.Invoke();
            }
        }
    }
}