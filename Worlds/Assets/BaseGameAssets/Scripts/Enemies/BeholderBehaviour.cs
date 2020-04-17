using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace World.Enemies
{
    public class BeholderBehaviour : MonoBehaviour
    {
        public float bounceRange = 1;
        public float bounceDistance = 1;
        public float bounceSpeed = 0.5f;

        private bool goingUp = false;

        private float hoverFromY = 1;

        private void Start()
        {
            hoverFromY = transform.position.y;

            StartCoroutine(UpdatePosition());
        }

        private void UpdateY()
        {
            Ray ray = new Ray(transform.position - transform.up, -transform.up);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo)) // hit an object
                hoverFromY = (hitInfo.point + (Vector3.one * bounceDistance)).y;

            Debug.DrawRay(ray.origin, ray.direction * hitInfo.distance);
        }

        private IEnumerator UpdatePosition()
        {
            while (true)
            {
                var pos = transform.position;

                if (pos.y > hoverFromY + (bounceRange / 2f))
                    goingUp = false;
                else if (pos.y < hoverFromY - (bounceRange / 2f))
                    goingUp = true;

                var bounceDist = goingUp ? bounceRange : -bounceRange;
                var d_pos = new Vector3(pos.x, pos.y + bounceDist, pos.z);
                transform.position = Vector3.Slerp(pos, d_pos, (Vector3.Distance(transform.position, d_pos)) * bounceSpeed * Time.deltaTime);

                yield return null;
            }
        }

        private void OnDrawGizmosSelected()
        {
            var top = transform.position + Vector3.down * (bounceRange / 2);
            var bot = transform.position + Vector3.up * (bounceRange / 2);

            Gizmos.DrawLine(top, bot);

            Gizmos.DrawCube(top, Vector3.one * 0.1f);
            Gizmos.DrawCube(bot, Vector3.one * 0.1f);
        }
    }
}