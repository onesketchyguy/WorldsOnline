using UnityEngine;

namespace Player
{
    public class PlayerAnimator : MonoBehaviour
    {
        public PlayerController controller;
        public Animator animator;
        private new Rigidbody rigidbody;

        [Header("Animations")]
        public string button1 = "Attack1";

        public string button2 = "Attack2";

        [Space]
        public string absSpeed = "Speed";

        private void Start()
        {
            if (controller == null)
                controller = GetComponent<PlayerController>();

            rigidbody = controller.GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if (controller == null)
            {
                Debug.LogError($"No player controller assigned to: {gameObject.name}:{nameof(PlayerAnimator)}");
                Destroy(this);
                return;
            }

            animator.SetBool(button1, controller.lastButton == LastButton.fire1);
            animator.SetBool(button2, controller.lastButton == LastButton.fire2);

            var val = (Mathf.Abs(rigidbody.velocity.x) + Mathf.Abs(rigidbody.velocity.z));
            animator.SetFloat(absSpeed, val);
        }
    }
}