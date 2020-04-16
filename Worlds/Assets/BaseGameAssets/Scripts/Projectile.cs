﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace World
{
    [RequireComponent(typeof(Rigidbody))]
    public class Projectile : NetworkBehaviour
    {
        private float damageToDeal;

        [Server]
        public void Launch(Vector3 dir, float force, float damageOnImpact)
        {
            damageToDeal = damageOnImpact;

            GetComponent<Rigidbody>().AddForce(dir * force, ForceMode.Force);
        }

        [ServerCallback]
        private void OnCollisionEnter(Collision collision)
        {
            // Check to see if we hit something damagable
            var other = collision.gameObject.GetComponent<HealthManager>();

            if (other != null)
                other.ModifyHealth(-damageToDeal);

            ObjectPool.localInstance.ReturnObject(gameObject);
        }
    }
}