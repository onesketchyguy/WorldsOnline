using Unity.Mathematics;
using UnityEngine;

namespace Player
{
    public class CharacterMovement : MonoBehaviour
    {
        public new Rigidbody rigidbody;

        public float speed = 5;
        public float runMultiplier = 1.65f;

        private IInput inputManager;

        private void SetInputManager()
        {
            inputManager = GetComponent<IInput>();

            if (inputManager == null)
            {
                Debug.LogError($"Unable to find IInput component on {gameObject.name}:{typeof(CharacterController)}");
                Destroy(this);
            }
        }

        private void Start()
        {
            SetInputManager();

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
            var invert = new float3(inputManager.axisInput.z, 0, inputManager.axisInput.x);
            invert = Vector3.Normalize(invert);

            var d_pos = new float3(invert.x, rigidbody.velocity.y, invert.z);

            var spd = inputManager.lastButton == LastButton.fire3 ? speed * runMultiplier : speed;
            rigidbody.velocity = Vector3.Lerp(rigidbody.velocity, d_pos * spd, spd * Time.deltaTime);
            rigidbody.angularVelocity = Vector3.zero;

            if (CompareFloat3(invert, lookVector) == false && (inputManager.axisInput.z != 0 || inputManager.axisInput.x != 0))
            {
                lookVector = invert;

                rigidbody.MoveRotation(Quaternion.LookRotation(lookVector));
            }
        }
    }
}