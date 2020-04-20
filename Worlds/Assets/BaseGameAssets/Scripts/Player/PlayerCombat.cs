using Mirror;
using UnityEngine;

namespace Worlds.Player
{
    public class PlayerCombat : InputReceiver
    {
        public Vector3 firePoint;

        public GameObject BulletPrefab;

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
                if (!firing) InvokeRepeating(nameof(CmdFire), 0, 15f * Time.deltaTime);
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

        [Command]
        private void CmdFire()
        {
            if (NetworkServer.active == false) return;

            firing = true;
            var projectile = ObjectManager.localInstance.GetObject(BulletPrefab, GetFirePoint());
            projectile.transform.localRotation = transform.rotation;

            projectile.GetComponent<Projectile>().RpcLaunch(transform.forward, 1000, 10);
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(GetFirePoint(), .25f);
            Gizmos.DrawRay(GetFirePoint(), transform.forward);
        }
    }
}