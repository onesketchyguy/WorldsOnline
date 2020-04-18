﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Networking
{
    public class ClientSend : MonoBehaviour
    {
        private static void SendTCPData(Packet packet)
        {
            packet.WriteLength();
            Client.instance.tcp.SendData(packet);
        }

        private static void SendUDPData(Packet packet)
        {
            packet.WriteLength();
            Client.instance.udp.SendData(packet);
        }

        public static void WelcomeRecieved()
        {
            using (var packet = new Packet((int)ClientPackets.welcomeReceived))
            {
                packet.Write(Client.instance.id);
                packet.Write(UIManager.instance.userNameField.text);

                SendTCPData(packet);
            }
        }

        public static void UDPTestRevieved()
        {
            using (var p = new Packet((int)ClientPackets.udpTestRecieved))
            {
                p.Write("Recieved a UDP packet");

                SendUDPData(p);
            }
        }
    }
}