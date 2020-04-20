using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

namespace Worlds.Player
{
    public class CharacterMovement : InputReceiver
    {
        public NavMeshAgent navAgent;

        public float speed = 5;
        public float runMultiplier = 1.65f;

        internal override void Start()
        {
            base.Start();

            if (navAgent == null)
            {
                navAgent = GetComponent<NavMeshAgent>() ?? gameObject.AddComponent<NavMeshAgent>();
            }
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

            var d_pos = (float3)transform.position + new float3(invert.x, 0, invert.z);

            var spd = inputManager.ButtonsContains(Button.fire3) ? speed * runMultiplier : speed;

            navAgent.speed = spd;
            navAgent.SetDestination(d_pos);

            if (CompareFloat3(invert, lookVector) == false && (inputManager.axisInput.z != 0 || inputManager.axisInput.x != 0))
            {
                lookVector = invert;
            }

            if (inputManager.ButtonsContains(Button.fire2) == false)
                transform.rotation = (Quaternion.LookRotation(lookVector));
        }
    }
}