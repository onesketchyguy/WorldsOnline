using Mirror;
using Mirror.Discovery;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Worlds.UI
{
    public class NetworkDiscoveryHUD : MonoBehaviour
    {
        private readonly Dictionary<long, ServerResponse> discoveredServers = new Dictionary<long, ServerResponse>();
        private Vector2 scrollViewPos = Vector2.zero;

        public NetworkDiscovery networkDiscovery;

        public UnityEngine.Events.UnityEvent onJoinServerEvent;
        public UnityEngine.Events.UnityEvent onHostServerEvent;

        public GameObject serverButtonPrefab;
        public Transform serverButtonParent;

        private void OnEnable()
        {
            RefreshServerList();
        }

        private void CreateServerButton(ServerResponse info)
        {
            var go = Instantiate(serverButtonPrefab, serverButtonParent);
            var text = go.GetComponentInChildren<Text>();
            text.alignment = TextAnchor.MiddleCenter;
            text.text = $"{info.serverHost}:{info.EndPoint.Address}";
            go.name = info.serverHost.ToString();

            go.GetComponentInChildren<Button>().onClick.AddListener(() => Connect(info));
        }

        public void ClearServerList()
        {
            discoveredServers.Clear();
            for (int i = 0; i < serverButtonParent.childCount; i++)
            {
                var child = serverButtonParent.GetChild(i);
                if (child.transform == serverButtonParent.transform)
                    continue;

                Destroy(child.gameObject);
            }
        }

        public void RefreshServerList()
        {
            ClearServerList();
            networkDiscovery.StartDiscovery();
        }

        public void StartServer()
        {
            ClearServerList();
            NetworkManager.singleton.StartServer();

            networkDiscovery.AdvertiseServer();
        }

        public void Host()
        {
            if (NetworkServer.localConnection != null)
            {
                // A local host exists connect to them instead of trying to host.
                NetworkManager.singleton.StartClient();

                if (onJoinServerEvent != null)
                    onJoinServerEvent.Invoke();

                return;
            }

            ClearServerList();
            NetworkManager.singleton.StartHost();
            networkDiscovery.AdvertiseServer();

            if (onHostServerEvent != null)
                onHostServerEvent.Invoke();
        }

        public void Connect(ServerResponse info)
        {
            NetworkManager.singleton.StartClient(info.uri);

            if (onJoinServerEvent != null)
                onJoinServerEvent.Invoke();
        }

        public void OnDiscoveredServer(ServerResponse info)
        {
            // Note that you can check the versioning to decide if you can connect to the server or not using this method

            if (discoveredServers.ContainsKey(info.serverId)) return;

            discoveredServers[info.serverId] = info;
            CreateServerButton(info);
        }
    }
}