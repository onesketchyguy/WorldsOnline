using UnityEngine;

namespace Worlds
{
    public class DropItemOnDeath : MonoBehaviour
    {
        public GameObject[] itemsToDrop;

        [Tooltip("The health to track.")]
        public HealthManager health;

        public Vector3 velocityToAdd = new Vector3(0, 1, 0);
        public float velocityRange = 0;

        private void Start()
        {
            health.onDeathCallback += () =>
            {
                // Create objects

                foreach (var item in itemsToDrop)
                {
                    var go = ObjectManager.GetObject(item, transform.position + Vector3.up);
                    var rigidBody = go.GetComponent<Rigidbody>();
                    if (rigidBody != null)
                    {
                        rigidBody.AddForce(velocityToAdd +
                            (Vector3.one * Random.Range(-velocityRange, velocityRange)), ForceMode.Impulse);
                    }
                }
            };
        }
    }
}