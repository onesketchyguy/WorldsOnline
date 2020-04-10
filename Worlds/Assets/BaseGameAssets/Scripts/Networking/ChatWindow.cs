using Mirror;
using Player;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace World
{
    public class ChatWindow : MonoBehaviour
    {
        public InputField chatMessage;
        public Text chatHistory;
        public Scrollbar scrollbar;

        public void Awake()
        {
            PlayerController.OnMessage += OnPlayerMessage;
        }

        private void OnPlayerMessage(PlayerController player, string message)
        {
            string prettyMessage;

            if (message.Contains("[LOGEVENT]"))
            {
                // Remove the phrase "[LOGEVENT]"
                var n_string = "";
                bool canAdd = true;
                for (int i = 0; i < message.Length; i++)
                {
                    var character = message[i];
                    if (character == '[')
                    {
                        canAdd = false;
                    }
                    else if (character == ']')
                    {
                        // We can add the NEXT character, but skip this one.
                        canAdd = true;
                        continue;
                    }

                    if (canAdd)
                        n_string += character;
                }

                message = n_string;
                prettyMessage = $"<color=red>{message} </color>";
            }
            else
            {
                prettyMessage = player.isLocalPlayer ?
                    $"<color=blue>{player.playerName}: </color> {message}" :
                    $"<color=green>{player.playerName}: </color> {message}";
            }

            AppendMessage(prettyMessage);

            Debug.Log(message);
        }

        public void OnSend()
        {
            if (chatMessage.text.Trim() == "")
                return;

            // get our player
            PlayerController player = NetworkClient.connection.identity.GetComponent<PlayerController>();

            // send a message
            player.CmdSend(chatMessage.text.Trim());

            chatMessage.text = "";
        }

        internal void AppendMessage(string message)
        {
            StartCoroutine(AppendAndScroll(message));
        }

        private IEnumerator AppendAndScroll(string message)
        {
            chatHistory.text += message + "\n";

            // it takes 2 frames for the UI to update ?!?!
            yield return null;
            yield return null;

            // slam the scrollbar down
            scrollbar.value = 0;
        }
    }
}