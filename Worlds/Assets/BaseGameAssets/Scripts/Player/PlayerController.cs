using UnityEngine;
using Mirror;
using Unity.Mathematics;

namespace Player
{
    public class PlayerController : NetworkBehaviour, IInput
    {
        public float3 axisInput { get; set; }
        public LastButton lastButton { get; set; }

        [SyncVar]
        public string playerName;

        public static event System.Action<PlayerController, string> OnMessage;

        [Command]
        public void CmdSend(string message)
        {
            if (message.Trim() != "")
                RpcReceive(message.Trim());
        }

        [ClientRpc]
        public void RpcReceive(string message)
        {
            OnMessage?.Invoke(this, message);
        }

        private void Start()
        {
            if (!isLocalPlayer)
                return;
            gameObject.name = playerName;

            CmdSend($"[LOGEVENT]{playerName} joined the game.");
        }

        private void Update()
        {
            if (!isLocalPlayer)
                return;

            axisInput = new float3(Input.GetAxis("Vertical"), 0, Input.GetAxis("Horizontal"));

            switch (lastButton)
            {
                case LastButton.none:
                    if (Input.GetButton("Fire1")) lastButton = LastButton.fire1;
                    if (Input.GetButton("Fire2")) lastButton = LastButton.fire2;
                    if (Input.GetButton("Fire3")) lastButton = LastButton.fire3;
                    break;

                case LastButton.fire1:
                    if (Input.GetButtonUp("Fire1")) lastButton = LastButton.none;
                    break;

                case LastButton.fire2:
                    if (Input.GetButtonUp("Fire2")) lastButton = LastButton.none;
                    break;

                case LastButton.fire3:
                    if (Input.GetButtonUp("Fire3")) lastButton = LastButton.none;
                    break;

                default:
                    break;
            }
        }
    }
}