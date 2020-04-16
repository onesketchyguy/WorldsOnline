using UnityEngine;
using Mirror;
using Unity.Mathematics;

namespace World.Player
{
    public class PlayerController : NetworkBehaviour, IInput
    {
        public float3 axisInput { get; set; }
        public Button[] latestButtons { get; set; }

        public bool ButtonsContains(Button button)
        {
            for (int i = 0; i < latestButtons.Length; i++)
            {
                if (latestButtons[i] == button) return true;
            }

            return false;
        }

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

            latestButtons = new Button[3];

            CmdSend($"[LOG]{playerName} joined the game.");
        }

        private void Update()
        {
            if (!isLocalPlayer)
                return;

            axisInput = new Vector3(Input.GetAxis("Vertical"), 0, Input.GetAxis("Horizontal")).normalized;
            UpdateButtons();
        }

        private void UpdateButtons()
        {
            if (Input.GetButtonDown("Fire1")) latestButtons[0] = Button.fire1;
            else if (Input.GetButtonUp("Fire1")) latestButtons[0] = Button.none;

            if (Input.GetButtonDown("Fire2")) latestButtons[1] = Button.fire2;
            else if (Input.GetButtonUp("Fire2")) latestButtons[1] = Button.none;

            if (Input.GetButtonDown("Fire3")) latestButtons[2] = Button.fire3;
            else if (Input.GetButtonUp("Fire3")) latestButtons[2] = Button.none;
        }
    }
}