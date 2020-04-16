using Unity.Mathematics;
using UnityEngine;

namespace World.Player
{
    public class CharacterMovement : InputReceiver
    {
        public new Rigidbody rigidbody;

        public float speed = 5;
        public float runMultiplier = 1.65f;

        internal override void Start()
        {
            base.Start();

            if (rigidbody == null)
                rigidbody = GetComponent<Rigidbody>() ?? gameObject.AddComponent<Rigidbody>();
        }

        private float3 lookVector;

        private bool CompareFloat3(float3 a, float3 b)
        {
            if (a.x != b.x) return false;
            if (a.y != b.y) return false;
            if (a.z != b.z) return false;

            return true;
        }

        private void Update()
        {
            var invert = new Vector3(inputManager.axisInput.z, 0, inputManager.axisInput.x).normalized;

            var d_pos = new float3(invert.x, rigidbody.velocity.y, invert.z);

            var spd = inputManager.ButtonsContains(Button.fire3) ? speed * runMultiplier : speed;
            rigidbody.velocity = Vector3.Lerp(rigidbody.velocity, d_pos * spd, spd * Time.deltaTime);
            rigidbody.angularVelocity = Vector3.zero;

            if (CompareFloat3(invert, lookVector) == false && (inputManager.axisInput.z != 0 || inputManager.axisInput.x != 0))
            {
                lookVector = invert;
            }

            if (inputManager.ButtonsContains(Button.fire2) == false)
                rigidbody.MoveRotation(Quaternion.LookRotation(lookVector));
        }
    }
}