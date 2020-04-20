using Mirror;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Worlds.Player;

namespace Worlds.UI
{
    public class ChatWindow : MonoBehaviour
    {
        public InputField chatMessage;
        public Text chatHistory;
        public Scrollbar scrollbar;

        public System.Action<PlayerController, string> OnRecievedPlayerMessage;

        public void Awake()
        {
            PlayerController.OnMessage += OnPlayerMessage;
        }

        private void OnPlayerMessage(PlayerController player, string message)
        {
            if (OnRecievedPlayerMessage != null)
                OnRecievedPlayerMessage.Invoke(player, message);
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
            if (message.Contains(':'))
            {
                var FullMessage = message.Split(':');

                if (FullMessage.Length > 1)
                {
                    chatHistory.text += FullMessage.FirstOrDefault();

                    foreach (var msg in FullMessage)
                    {
                        if (msg == FullMessage.FirstOrDefault()) continue;
                        chatHistory.text += ':';

                        for (int i = 0; i < msg.Length; i++)
                        {
                            yield return null;
                            chatHistory.text += msg[i];
                        }
                    }
                }
            }
            else
            {
                chatHistory.text += message;
            }

            chatHistory.text += "\n";

            // it takes 2 frames for the UI to update ?!?!
            yield return null;
            yield return null;

            // slam the scrollbar down
            scrollbar.value = 0;
        }
    }
}