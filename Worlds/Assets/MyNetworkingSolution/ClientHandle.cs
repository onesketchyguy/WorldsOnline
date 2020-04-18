using System.Net;
using UnityEngine;

namespace Networking
{
    public class ClientHandle : MonoBehaviour
    {
        public static void Welcome(Packet packet)
        {
            string message = packet.ReadString();
            int clientId = packet.ReadInt();

            Debug.Log($"Message sent from server: {message}");

            Client.instance.id = clientId;
            ClientSend.WelcomeRecieved();

            Client.instance.udp.Connect(((IPEndPoint)Client.instance.tcp.socket.Client.LocalEndPoint).Port);
        }

        public static void UDPTest(Packet P)
        {
            string message = P.ReadString();

            Debug.Log($"Recieved packed via UDP... {message}");
            ClientSend.UDPTestRevieved();
        }
    }
}