using UnityEngine;
using Mirror;
using Worlds.Player;
using UI;

namespace Worlds.UI
{
    public class ChatManager : NetworkBehaviour
    {
        public ChatWindow chatWindow;

        [SyncVar]
        public string chatLog;

        public override void OnStartServer()
        {
            base.OnStartServer();
        }

        public override void OnStartClient()
        {
            base.OnStartClient();

            if (chatLog != null && chatLog.Length > 0)
                chatWindow.AppendMessage(chatLog);
            chatWindow.OnRecievedPlayerMessage += OnPlayerMessage;
        }

        private void OnPlayerMessage(PlayerController player, string message)
        {
            string prettyMessage;

            if (message.ToLower().Contains("[log]"))
            {
                // Remove the phrase "[LOG]"
                var n_string = "";
                bool canAdd = true;
                for (int i = 0; i < message.Length; i++)
                {
                    var character = message[i];

                    if (character == ']' && canAdd == false)
                    {
                        // We can add the NEXT character, but skip this one.
                        canAdd = true;
                        continue;
                    }
                    else if (canAdd == false) continue;
                    else if (character == '[')
                    {
                        canAdd = false;
                        continue;
                    }

                    n_string += character;
                }

                message = n_string;
                prettyMessage = $"{message}";
            }
            else
            {
                prettyMessage = (player.isLocalPlayer ? $"<color=Green>{player.playerName}</color>" : $"<color=Blue>{player.playerName}</color>")
                    + $":{message}";
            }

            chatWindow.AppendMessage(prettyMessage);
            FindObjectOfType<NotificationManager>().CreateMessage(prettyMessage, 3, Fonts.Default, Backgrounds.none, Icons.none);
            chatLog += $"{prettyMessage}\n";
        }
    }
}