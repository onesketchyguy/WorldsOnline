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
            string prettyMessage = player.isLocalPlayer ?
                $"<color=red>{player.playerName}: </color> {message}" :
                $"<color=blue>{player.playerName}: </color> {message}";
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