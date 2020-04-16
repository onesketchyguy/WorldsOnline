using Mirror;
using UnityEngine;

namespace World
{
    using Player;

    public class WorldNetworkManager : NetworkManager
    {
        public UnityEngine.Events.UnityEvent OnDisconnectEvent;

        public string DefaultUserName = "USER";

        public string PlayerName { get; set; }

        public void SetHostname(string hostname)
        {
            networkAddress = hostname;
        }

        public ChatWindow chatWindow;

        public class CreatePlayerMessage : MessageBase
        {
            public string name;
        }

        public override void OnStartServer()
        {
            base.OnStartServer();
            NetworkServer.RegisterHandler<CreatePlayerMessage>(OnCreatePlayer);
        }

        public override void OnClientConnect(NetworkConnection conn)
        {
            base.OnClientConnect(conn);

            // tell the server to create a player with this name
            conn.Send(new CreatePlayerMessage { name = (PlayerName != null && PlayerName.Length > 0) ? PlayerName : DefaultUserName });
        }

        public override void OnClientDisconnect(NetworkConnection conn)
        {
            base.OnClientDisconnect(conn);

            if (conn.identity.isLocalPlayer)
            {
                if (OnDisconnectEvent != null)
                    OnDisconnectEvent.Invoke();
            }
        }

        public override void OnServerDisconnect(NetworkConnection conn)
        {
            base.OnServerDisconnect(conn);

            if (OnDisconnectEvent != null)
                OnDisconnectEvent.Invoke();
        }

        private void OnCreatePlayer(NetworkConnection connection, CreatePlayerMessage createPlayerMessage)
        {
            // create a gameobject using the name supplied by client
            GameObject playergo = Instantiate(playerPrefab);
            playergo.GetComponent<PlayerController>().playerName = createPlayerMessage.name;

            Enemies.EnemyBlackBoard.players.Add(playergo.transform);

            // set it as the player
            NetworkServer.AddPlayerForConnection(connection, playergo);

            chatWindow.gameObject.SetActive(true);
        }
    }
}