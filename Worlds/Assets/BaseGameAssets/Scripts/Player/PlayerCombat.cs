using Mirror;
using UnityEngine;

namespace World.Player
{
    public class PlayerCombat : InputReceiver
    {
        public Vector3 firePoint;

        public GameObject BulletPrefab;

        private Vector3 GetFirePoint()
        {
            return transform.position + transform.TransformVector(firePoint);
        }

        private bool firing;

        private void Update()
        {
            // Shoot
            if (inputManager.ButtonsContains(Button.fire1))
            {
                if (!firing)
                    InvokeRepeating(nameof(CmdFire), 0, 0.25f);
            }
            else
            {
                CancelInvoke(nameof(CmdFire));
                firing = false;
            }

            // Look
            if (inputManager.ButtonsContains(Button.fire2))
            {
                var lookAt = transform.position;

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitInfo;
                if (Physics.Raycast(ray, out hitInfo)) // hit an object
                {
                    lookAt = hitInfo.point - transform.position;
                }

                lookAt.y = transform.position.y;

                transform.rotation = (Quaternion.LookRotation(lookAt));
            }
        }

        [Command]
        private void CmdFire()
        {
            firing = true;
            var projectile = ObjectPool.localInstance.GetObject(BulletPrefab, GetFirePoint());
            projectile.transform.localRotation = transform.rotation;

            projectile.GetComponent<Projectile>().Launch(transform.forward, 1000, 10);
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(GetFirePoint(), .25f);
            Gizmos.DrawRay(GetFirePoint(), transform.forward);
        }
    }
}