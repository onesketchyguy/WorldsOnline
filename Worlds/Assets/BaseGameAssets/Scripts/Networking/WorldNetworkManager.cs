using Mirror;
using UnityEngine;

namespace Worlds
{
    using Player;
    using UI;

    public class WorldNetworkManager : NetworkManager
    {
        public UnityEngine.Events.UnityEvent OnDisconnectEvent;

        public string DefaultUserName = "USER";

        public string PlayerName { get; set; }

        public void SetHostname(string hostname)
        {
            networkAddress = hostname;
        }

        internal Vector3 playerSpawnPoint;

        public ChatWindow chatWindow;

        internal Stats localPlayerStats = new Stats()
        {
            Dexterity = new Stat(5),
            Strength = new Stat(5),
            Intelligence = new Stat(5),
            Constitution = new Stat(5)
        };

        public class CreatePlayerMessage : MessageBase
        {
            public string name;
            public int Strength;
            public int Intelligence;
            public int Dexterity;
            public int Constitution;
        }

        public override void OnStartServer()
        {
            base.OnStartServer();
            NetworkServer.RegisterHandler<CreatePlayerMessage>(OnCreatePlayer);
        }

        public override void OnClientConnect(NetworkConnection conn)
        {
            base.OnClientConnect(conn);

            PlayerName = (PlayerName != null && PlayerName.Length > 0) ? PlayerName : DefaultUserName;

            if (conn.address == NetworkServer.localConnection.address)
                if (transport != null)
                    transport.hostName = PlayerName;

            // tell the server to create a player with this name
            conn.Send(new CreatePlayerMessage
            {
                name = PlayerName,
                Strength = localPlayerStats.Strength.value,
                Intelligence = localPlayerStats.Intelligence.value,
                Dexterity = localPlayerStats.Dexterity.value,
                Constitution = localPlayerStats.Constitution.value
            });
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
            GameObject playergo = Instantiate(playerPrefab, playerSpawnPoint, Quaternion.identity);
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