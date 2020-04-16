using Mirror;
using UnityEngine;

namespace World.Player
{
    public class PlayerAnimator : InputReceiver
    {
        public Animator animator;
        private new Rigidbody rigidbody;

        [Header("Animations")]
        public string button1 = "Attack1";

        public string button2 = "Attack2";

        [Space]
        public string absSpeed = "Speed";

        private void Update()
        {
            if (!isLocalPlayer) return;

            if (inputManager == null)
            {
                Debug.LogError($"No player controller assigned to: {gameObject.name}:{nameof(PlayerAnimator)}");
                Destroy(this);
                return;
            }

            CmdSetBool(button1, inputManager.ButtonsContains(Button.fire1));
            CmdSetBool(button2, inputManager.ButtonsContains(Button.fire2));

            var val = (Mathf.Abs(inputManager.axisInput.x) + Mathf.Abs(inputManager.axisInput.z));
            CmdSetFloat(absSpeed, val * 0.5f);
        }

        /// <summary>
        /// Tell the server to set a bool
        /// </summary>
        /// <param name="_float"></param>
        /// <param name="value"></param>
        [Command]
        private void CmdSetBool(string _bool, bool value) => RpcSetBool(_bool, value);

        /// <summary>
        /// Tell the server to set a float
        /// </summary>
        /// <param name="_float"></param>
        /// <param name="value"></param>
        [Command]
        private void CmdSetFloat(string _float, float value) => RpcSetFloat(_float, value);

        /// <summary>
        /// Tell the client to set a bool
        /// </summary>
        /// <param name="_float"></param>
        /// <param name="value"></param>
        [ClientRpc]
        private void RpcSetBool(string _bool, bool value) => animator.SetBool(_bool, value);

        /// <summary>
        /// Tell the client to set a float
        /// </summary>
        /// <param name="_float"></param>
        /// <param name="value"></param>
        [ClientRpc]
        private void RpcSetFloat(string _float, float value) => animator.SetFloat(_float, value);
    }
}