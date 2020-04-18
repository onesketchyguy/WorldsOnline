using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace Networking
{
    public class Client : MonoBehaviour
    {
        public static Client instance;
        public static int dataBufferSize = 4096;

        public string ip = "127.0.0.1";
        public int port = 2800;
        public int id = 0;
        public TCP tcp;
        public UDP udp;

        private delegate void PacketHandler(Packet packet);

        private static Dictionary<int, PacketHandler> packetHandlers;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(this);
            }
        }

        private void Start()
        {
            tcp = new TCP();
            udp = new UDP();
        }

        public void ConnectToServer()
        {
            InitializeClientData();
            tcp.Connect();
        }

        public class TCP
        {
            public TcpClient socket;
            private Packet recievedData;
            private NetworkStream stream;
            private byte[] recieveBuffer;

            public void Connect()
            {
                socket = new TcpClient
                {
                    SendBufferSize = dataBufferSize,
                    ReceiveBufferSize = dataBufferSize
                };

                recieveBuffer = new byte[dataBufferSize];
                socket.BeginConnect(instance.ip, instance.port, ConnectCallback, socket);
            }

            private void ConnectCallback(IAsyncResult result)
            {
                socket.EndConnect(result);

                if (!socket.Connected) return;

                stream = socket.GetStream();

                recievedData = new Packet();

                stream.BeginRead(recieveBuffer, 0, dataBufferSize, RecieveCallback, null);
            }

            private void RecieveCallback(IAsyncResult result)
            {
                try
                {
                    int byteLength = stream.EndRead(result);
                    if (byteLength <= 0)
                    {
                        // TODO: Disconnect
                        return;
                    }

                    byte[] data = new byte[byteLength];
                    Array.Copy(recieveBuffer, data, byteLength);

                    recievedData.Reset(HandleData(data));
                    stream.BeginRead(recieveBuffer, 0, dataBufferSize, RecieveCallback, null);
                }
                catch (Exception exception)
                {
                    Debug.LogError($"Error recieving TCP data: {exception}");
                    // TODO: Disconnect
                }
            }

            private bool HandleData(byte[] data)
            {
                int packetLength = 0;
                recievedData.SetBytes(data);
                var unreadDataLength = recievedData.UnreadLength();

                if (unreadDataLength >= 4)
                {
                    packetLength = recievedData.ReadInt();
                    if (packetLength <= 1)
                    {
                        return true;
                    }
                }

                while (packetLength > 0 && packetLength <= unreadDataLength)
                {
                    var packedBytes = recievedData.ReadBytes(packetLength);

                    ThreadManager.ExecuteOnMainThread(() =>
                    {
                        using (Packet packet = new Packet(packedBytes))
                        {
                            int packetId = packet.ReadInt();
                            packetHandlers[packetId](packet);
                        }
                    });

                    packetLength = 0;
                    unreadDataLength = recievedData.UnreadLength();

                    if (unreadDataLength >= 4)
                    {
                        packetLength = recievedData.ReadInt();
                        if (packetLength <= 1)
                        {
                            return true;
                        }
                    }
                }

                if (packetLength <= 1)
                    return true;
                else return false;
            }

            public void SendData(Packet packet)
            {
                try
                {
                    if (socket != null)
                    {
                        stream.BeginWrite(packet.ToArray(), 0, packet.Length(), null, null);
                    }
                }
                catch (Exception exception)
                {
                    Debug.LogError($"Unable to send data to server via TCP: {exception}");
                }
            }
        }

        public class UDP
        {
            public UdpClient socket;
            public IPEndPoint endPoint;

            public UDP()
            {
                endPoint = new IPEndPoint(IPAddress.Parse(instance.ip), instance.port);
            }

            public void Connect(int localPort)
            {
                socket = new UdpClient(localPort);
                socket.Connect(endPoint);
                socket.BeginReceive(RecieveCallback, null);

                using (var packet = new Packet())
                {
                    SendData(packet);
                }
            }

            public void SendData(Packet packet)
            {
                try
                {
                    packet.InsertInt(instance.id);

                    if (socket != null)
                    {
                        socket.BeginSend(packet.ToArray(), packet.Length(), null, null);
                    }
                }
                catch (Exception exception)
                {
                    Debug.LogError($"Error sending data to server via UDP {exception}");
                }
            }

            private void RecieveCallback(IAsyncResult result)
            {
                try
                {
                    var data = socket.EndReceive(result, ref endPoint);
                    socket.BeginReceive(RecieveCallback, null);

                    if (data.Length < 4)
                        return;

                    HandleData(data);
                }
                catch (Exception exception)
                {
                    Debug.LogError($"{exception}");
                }
            }

            private void HandleData(byte[] data)
            {
                using (var packet = new Packet(data))
                {
                    int packetLength = packet.ReadInt();
                    data = packet.ReadBytes(packetLength);
                }

                ThreadManager.ExecuteOnMainThread(() =>
                {
                    using (var packet = new Packet(data))
                    {
                        int packetID = packet.ReadInt();
                        packetHandlers[packetID](packet);
                    }
                });
            }
        }

        public void InitializeClientData()
        {
            packetHandlers = new Dictionary<int, PacketHandler>()
        {
            { (int)ServerPackets.welcome, ClientHandle.Welcome },
            { (int)ServerPackets.udpTest, ClientHandle.UDPTest },
        };

            Debug.Log("Initialized packets.");
        }
    }
}