using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Worlds.Items
{
    [RequireComponent(typeof(Collider))]
    public class ItemPickup : MonoBehaviour
    {
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                var player = collision.gameObject.GetComponent<Player.PlayerController>();
                // Add item to player inventory
                // TODO

                // Remove this object from the world
                ObjectManager.localInstance.ReturnObject(gameObject);
            }
        }
    }
}