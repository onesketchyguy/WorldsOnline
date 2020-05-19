using Mirror;
using UnityEngine;

namespace Worlds
{
    using Player;
    using System;
    using UI;

    public class WorldNetworkManager : NetworkManager
    {
        public UnityEngine.Events.UnityEvent OnConnectEvent;
        public UnityEngine.Events.UnityEvent OnDisconnectEvent;

        public string DefaultUserName = "USER";

        public string PlayerName { get; set; }

        public void SetHostname(string hostname)
        {
            networkAddress = hostname;
        }

        public ChatWindow chatWindow;

        public CreatePlayerMessage playerData = new CreatePlayerMessage();

        public class CreatePlayerMessage : MessageBase
        {
            public string name = "USER";
            public int Strength = 5;
            public int Intelligence = 5;
            public int Dexterity = 5;
            public int Constitution = 5;
            public Vector3 position;
        }

        public override void OnStartServer()
        {
            base.OnStartServer();
            NetworkServer.RegisterHandler<CreatePlayerMessage>(OnCreatePlayer);
        }

        public override void OnClientConnect(NetworkConnection conn)
        {
            base.OnClientConnect(conn);

            if (OnConnectEvent != null)
                OnConnectEvent.Invoke();

            // tell the server to create a player with this name
            PlayerName = (PlayerName != null && PlayerName.Length > 0) ? PlayerName : DefaultUserName;

            playerData.name = PlayerName;
            conn.Send(playerData);
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

        private void OnCreatePlayer(NetworkConnection connection, CreatePlayerMessage playerMessage)
        {
            // create a gameobject using the name supplied by client
            GameObject playergo = Instantiate(playerPrefab, playerMessage.position, Quaternion.identity);
            var character = playergo.GetComponent<PlayerController>();
            character.playerName = playerMessage.name;
            character.m_stats.stats = new Stats()
            {
                Strength = new Stat(playerMessage.Strength),
                Constitution = new Stat(playerMessage.Constitution),
                Intelligence = new Stat(playerMessage.Intelligence),
                Dexterity = new Stat(playerMessage.Dexterity)
            };

            Enemies.EnemyBlackBoard.players.Add(playergo.transform);

            // set it as the player
            NetworkServer.AddPlayerForConnection(connection, playergo);

            chatWindow.gameObject.SetActive(true);
        }
    }
}