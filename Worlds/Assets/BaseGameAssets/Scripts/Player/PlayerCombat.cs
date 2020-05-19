using Mirror;
using System.Collections;
using UnityEngine;

namespace Worlds.Player
{
    public class PlayerCombat : InputReceiver
    {
        public int damage = 10;

        public Vector3 firePoint;

        public GameObject BulletPrefab;

        public LayerMask attackableLayers;
        public float attackRange = 1.5f;

        //public float attackDelay = 0.25f;

        private Vector3 GetFirePoint()
        {
            return transform.position + transform.TransformVector(firePoint);
        }

        [SyncVar]
        private bool firing;

        private void Update()
        {
            // Shoot
            if (inputManager.ButtonsContains(Button.fire1))
            {
                if (!firing)
                {
                    firing = true;
                    //   StartCoroutine(FireWeapon());
                }
            }
            else
            {
                //StopCoroutine(FireWeapon());
                if (firing)
                {
                    // Moved to animator
                    //Invoke(nameof(CmdFireRay), attackDelay);
                    firing = false;
                }
            }

            // Look
            if (inputManager.ButtonsContains(Button.fire2))
            {
                var lookAt = transform.position;

                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitInfo;
                if (Physics.Raycast(ray, out hitInfo)) // hit an object
                {
                    lookAt = hitInfo.point;
                    lookAt.y = transform.position.y;
                }

                transform.LookAt(lookAt);
            }
        }

        private IEnumerator FireWeapon()
        {
            firing = true;
            yield return null;
            if (NetworkServer.active == false) firing = false;

            while (firing)
            {
                CmdFireProjectile();
                yield return new WaitForSecondsRealtime(0.15f);
            }
        }

        [Command]
        private void CmdFireProjectile()
        {
            var projectile = ObjectManager.GetObject(BulletPrefab, GetFirePoint());
            projectile.transform.localRotation = transform.rotation;

            projectile.GetComponent<Projectile>().RpcLaunch(transform.forward, 1000, 10);
        }

        [Command]
        public void CmdFireRay()
        {
            var capsuleCast = Physics.SphereCastAll(GetFirePoint(), 0.35f, transform.forward, attackRange, attackableLayers, QueryTriggerInteraction.UseGlobal);

            if (capsuleCast != null)
            {
                foreach (var hitInfo in capsuleCast)
                {
                    if (hitInfo.transform != null)
                    {
                        var collision = hitInfo.transform.gameObject;

                        // Check to see if we hit something damagable
                        var other = collision.GetComponent<HealthManager>();

                        if (other != null) other.RpcModifyHealth(-damage);

                        Debug.DrawRay(GetFirePoint(), transform.forward * hitInfo.distance, Color.red, 1);
                    }
                    else Debug.DrawRay(GetFirePoint(), transform.forward * attackRange, Color.black, 1);
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(GetFirePoint(), .25f);
            Gizmos.DrawRay(GetFirePoint(), transform.forward);
        }
    }
}