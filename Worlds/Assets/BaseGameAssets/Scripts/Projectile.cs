using Mirror;
using UnityEngine;

namespace Worlds
{
    [RequireComponent(typeof(Rigidbody))]
    public class Projectile : NetworkBehaviour
    {
        private float damageToDeal;

        [ClientRpc]
        public void RpcLaunch(Vector3 dir, float force, float damageOnImpact)
        {
            damageToDeal = damageOnImpact;

            GetComponent<Rigidbody>().AddForce(dir * force, ForceMode.Force);
        }

        [ServerCallback]
        private void OnCollisionEnter(Collision collision)
        {
            // Check to see if we hit something damagable
            var other = collision.gameObject.GetComponent<HealthManager>();

            if (other != null) other.RpcModifyHealth(-damageToDeal);

            ObjectManager.ReturnObject(gameObject);
        }
    }
}