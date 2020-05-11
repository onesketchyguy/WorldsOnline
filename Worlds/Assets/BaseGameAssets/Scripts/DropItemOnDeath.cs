using UnityEngine;

namespace Worlds
{
    public class DropItemOnDeath : MonoBehaviour
    {
        public GameObject[] itemsToDrop;

        [Tooltip("The health to track.")]
        public HealthManager health;

        private void Start()
        {
            health.onDeathCallback += () =>
            {
                // Create objects

                foreach (var item in itemsToDrop)
                {
                    ObjectManager.localInstance.GetObject(item, transform.position + Vector3.up);
                }
            };
        }
    }
}